using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using MEC;

public class AnomalyDetectionServerManager : MonoBehaviour
{
    [Header("Write it in this format http://127.0.0.1:5000 the server address")]
    public string serverAddress = "http://127.0.0.1:5000/";
    private string trainEndpoint = "http://127.0.0.1:5000/train";
    private string predictEndpoint = "http://127.0.0.1:5000/predict";
    private bool isPredictionInProgress = false;

    private void Awake()
    {
        trainEndpoint = $"{serverAddress}/train";
        predictEndpoint = $"{serverAddress}/predict";
    }

    public void TrainModel(List<float> dataset)
    {
        Timing.RunCoroutine(TrainModelCoroutine(dataset));
    }
    
    public void TryToPredictAnomaly(float targetSpeed, Action<int> callback)
    {
        if (isPredictionInProgress)
        {
            Debug.LogWarning("Prediction is already in progress. Ignoring new prediction request.");
            callback(-1);
            return;
        }
        
        Timing.RunCoroutine(PredictAnomalyCoroutine(targetSpeed, callback));
    }

    #region Coroutines

    private IEnumerator<float> TrainModelCoroutine(List<float> dataset)
    {
        using (UnityWebRequest trainRequest = UnityWebRequest.PostWwwForm(trainEndpoint, "POST"))
        {
            // Prepare data for training
            string jsonPayload = JsonConvert.SerializeObject(new { player_speed_data = dataset });
            
            // Configure the UnityWebRequest
            trainRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonPayload));
            trainRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return Timing.WaitUntilDone(trainRequest.SendWebRequest());

            // Check for errors
            if (trainRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error training model: " + trainRequest.error);
            }
            else
            {
                Debug.Log("Model trained successfully");
            }
            trainRequest.Dispose();
        }
    }
   
    private IEnumerator<float> PredictAnomalyCoroutine(float dataToCheck, Action<int> callback)
    {
        isPredictionInProgress = true;

        using (UnityWebRequest predictRequest = UnityWebRequest.PostWwwForm(predictEndpoint, "POST"))
        {
            // Prepare data for prediction
            var dataDict = new Dictionary<string, float> { { "current_speed", dataToCheck } };

            // Serialize the data to JSON
            string jsonPayload = JsonConvert.SerializeObject(dataDict);

            // Configure the UnityWebRequest
            predictRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonPayload));
            predictRequest.downloadHandler = new DownloadHandlerBuffer();
            predictRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return Timing.WaitUntilDone(predictRequest.SendWebRequest());

            // Check for errors
            if (predictRequest.result == UnityWebRequest.Result.Success)
            {
                // Deserialize the response
                string responseJson = predictRequest.downloadHandler.text;
                AnomalyResponse response = JsonConvert.DeserializeObject<AnomalyResponse>(responseJson);
                int prediction = response.anomalyPrediction;
                callback(prediction);
            }
            else
            {
                Debug.LogError("Error predicting anomalies: " + predictRequest.error);
                callback(-1); // Return -1 in case of error
            }
            
            predictRequest.Dispose();
        }
        
        // Clear the callback reference after use
        callback = null;
        
        isPredictionInProgress = false;
    }
    
    #endregion

}

[System.Serializable]
public class AnomalyResponse
{
    public int anomalyPrediction;
}
