# AnomalyDetection
Implementation of anomaly detection in Unity, using Python's Isolation Forest algorithm, offers a foundational understanding of detecting player speed anomalies during gameplay. While basic in nature, it serves as an introduction to the concept, paving the way for more sophisticated implementations in the future.

In the PlayerController script:

Initially disable anomaly detection to allow the system to gather data.
Play the game for a while to accumulate data for the anomaly detection system.
Stop the game and restart it to enable anomaly detection with the gathered data for learning purposes.

To use this implementation:

Run the Python anomaly detection server by executing the provided scripts.
Note the local server address (usually http://127.0.0.1:5000).
Open the Unity project and navigate to the AnomalyDetectionServerManager object in the scene.
Replace the server address variables in the object inspector with the server address shown in the python server script once you run it.
With the connection established, you're ready to detect player speed anomalies in your Unity game.
