from sklearn.ensemble import IsolationForest
import numpy as np

class IsolationForestAnomalyDetector:
    def __init__(self, contamination=0.1):  # Adjust the contamination parameter
        # Initialize an empty list to store historical data
        self.historical_data = []
        # Initialize the Isolation Forest model
        self.model = IsolationForest(contamination=contamination)  # Set contamination parameter

    def train_model(self):
        if len(self.historical_data) > 0:
            # Convert historical data to numpy array
            X = np.array(self.historical_data).reshape(-1, 1)
            # Train the Isolation Forest model
            self.model.fit(X)

    def update_model(self, new_data):
        # Add new data to historical data
        self.historical_data.extend(new_data)
        # Retrain the model with updated data
        self.train_model()

    def predict(self, curr_speed):
        # Convert current speed to numpy array
        X_test = np.array(curr_speed).reshape(-1, 1)
        # Predict if the speed is an anomaly (1 = inlier, -1 = outlier)
        prediction = self.model.predict(X_test)
        # Convert prediction to boolean (True if anomaly, False if not)
        is_anomaly = prediction == -1
        # Convert boolean to int (1 if anomaly, 0 if not)
        is_anomaly_int = is_anomaly.astype(int)
        return is_anomaly_int.tolist()  # Convert to serializable list
