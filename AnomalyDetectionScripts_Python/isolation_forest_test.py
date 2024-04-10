from isolation_forest import IsolationForestAnomalyDetector

# Example player speed data
player_speed_data = [1.490116, 1.617848, 3.125978, 4.66034, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.921, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 0.0, 1.532747, 3.047976, 4.578482, 5.0, 5.0, 5.0, 5.0, 4.92152, 4.92152, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 5.0, 4.92152, 5.0, 5.0, 5.0, 0.7055536, 2.238762, 3.77679, 5.0, 5.0, 5.0, 5.0, 4.92152, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 4.92152, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 4.221445, 2.703042, 0.0393885, 1.572605, 3.102242, 4.622152, 0.467202, 1.921476, 3.524921, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 4.92152, 1.490116e-08, 1.588309, 3.123314, 4.586532, 3.800769, 2.273439, 0.7454247, 0.0, 1.569896, 3.045671, 4.555788, 0.0, 1.576441, 3.084311, 4.549485, 3.529777, 2.005737, 0.4776146, 0.0, 1.522683, 3.057474, 4.572408, 1.388979, 2.92353, 4.423088, 0.7684711, 0.2535812, 0.049317, 4.649036, 1.466835, 2.97805, 4.547498, 1.686316, 4.720159, 3.438241, 3.922874, 4.284753, 2.799497, 1.266188, 3.893235, 2.411482, 0.9034551]

# Create an instance of AnomalyDetector
anomaly_detector = IsolationForestAnomalyDetector(0.3)

# Train the model with player speed data
anomaly_detector.update_model(player_speed_data)

# Predict anomalies for a current speed value
current_speed = 3
is_anomaly = anomaly_detector.predict(current_speed)

print(f"Is anomaly for speed {current_speed}: {is_anomaly}")

current_speed = 4.8
is_anomaly = anomaly_detector.predict(current_speed)
print(f"Is anomaly for speed {current_speed}: {is_anomaly}")

current_speed = 20
is_anomaly = anomaly_detector.predict(current_speed)
print(f"Is anomaly for speed {current_speed}: {is_anomaly}")

current_speed = 15
is_anomaly = anomaly_detector.predict(current_speed)
print(f"Is anomaly for speed {current_speed}: {is_anomaly}")

current_speed = 6
is_anomaly = anomaly_detector.predict(current_speed)
print(f"Is anomaly for speed {current_speed}: {is_anomaly}")

current_speed = 2
is_anomaly = anomaly_detector.predict(current_speed)
print(f"Is anomaly for speed {current_speed}: {is_anomaly}")