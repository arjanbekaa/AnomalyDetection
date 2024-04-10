using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnomalyDetectionManager : MonoSingleton<AnomalyDetectionManager>
{
    [FormerlySerializedAs("_anomalyDetectionClient")] public AnomalyDetectionServerManager anomalyDetectionServerManager;

    public List<float> newDataToAdd = new List<float>();

    public override void Init()
    {
        persistOnSceneLoad = false;
        base.Init();
    }

    public void TrainModelWithData(List<float> dataset)
    {
        if(dataset.Count <= 0)
            return;
        
        anomalyDetectionServerManager.TrainModel(dataset);
    }
    
    public void PredictIfSpeedIsAnomaly(PlayerData playerData)
    {
        anomalyDetectionServerManager.TryToPredictAnomaly(playerData.PlayerSpeed, result =>
        {
            if(result == -1)
                // Connection Failed
                return;

            bool isAnomaly = result == 1;
            if (!isAnomaly)
            {
                newDataToAdd.Add(playerData.PlayerSpeed);
                DataManager.instance.UpdatePlayerData(playerData.PlayerId, playerData.PlayerName, playerData.Time, playerData.PlayerX, playerData.PlayerY, playerData.PlayerSpeed);

                if (newDataToAdd.Count >= 50)
                {
                    TrainModelWithData(new List<float>(newDataToAdd));
                    newDataToAdd.Clear();
                }
            }

            /*
             DataManager.instance.AddPlayerAnomalyData(new PlayerAnomalyData()
            {
                playerSpeed = playerData.PlayerSpeed,
                isAnomaly = result,
            });
            */

            Debug.Log($"Target speed {playerData.PlayerSpeed} is anomaly: {isAnomaly}");
        });
    }
}
