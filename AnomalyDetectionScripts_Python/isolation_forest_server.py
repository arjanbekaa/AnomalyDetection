from flask import Flask, request, jsonify
from isolation_forest import IsolationForestAnomalyDetector

app = Flask(__name__)
anomaly_detector = IsolationForestAnomalyDetector(0.3)

# File path to save the speed and prediction data
data_file_path = "E:\\_Git-Projects\\LearningPython\\IsolationForestPython\\AnomalyDetectionScripts\\Data\\player_anomaly_data.txt"

@app.route('/train', methods=['POST'])
def train_model():
    print("Received train request")
    try:
        # Parse JSON data from the request
        data = request.get_json()
        player_speed_data = data['player_speed_data']
        print("player_speed_data:", player_speed_data)
        anomaly_detector.update_model(player_speed_data)
        return 'Model trained successfully', 200
    except Exception as e:
        print("Error training model:", str(e))
        return 'Failed to train model', 400

@app.route('/predict', methods=['POST'])
def predict():
    print("Received predict request")
    try:
        # Parse JSON data from the request
        data = request.get_json()
        current_speed = data['current_speed']
        prediction = anomaly_detector.predict(current_speed)
        print(f"Is anomaly for speed {current_speed}: {prediction}")

        # Save the speed and prediction to the file
        with open(data_file_path, 'a') as file:
            file.write(f"\n{current_speed},{prediction[0]}")

        return jsonify({'anomalyPrediction': prediction[0]}), 200
    except Exception as e:
        print("Error predicting anomalies:", str(e))
        return 'Failed to predict anomalies', 400

if __name__ == '__main__':
    app.run(debug=True)
