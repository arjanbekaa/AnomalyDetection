import matplotlib.pyplot as plt

# Path to the text file containing anomaly data
file_path = "E:\\_Git-Projects\\LearningPython\\IsolationForestPython\\AnomalyDetectionScripts\\Data\\player_anomaly_data.txt"


# Read the anomaly data from the text file
def read_anomaly_data(file_path):
    anomaly_data = []
    try:
        with open(file_path, 'r') as file:
            for line in file:
                line = line.strip()
                if line:  # Check if the line is not empty
                    values = line.split(',')
                    if len(values) == 2:
                        player_speed, is_anomaly = values
                        anomaly_data.append({'playerSpeed': float(player_speed), 'isAnomaly': int(is_anomaly)})
                    else:
                        print("Invalid data format in line:", line)
                else:
                    print("Empty line found.")
        return anomaly_data
    except Exception as e:
        print("Error reading anomaly data:", str(e))
        return None

# Fetch the anomaly data from the file
anomaly_data = read_anomaly_data(file_path)

if anomaly_data:
    # Count the occurrences of anomalies and non-anomalies for each speed
    anomaly_counts = {}
    for data in anomaly_data:
        speed = data['playerSpeed']
        is_anomaly = data['isAnomaly']
        if speed not in anomaly_counts:
            anomaly_counts[speed] = {'anomaly': 0, 'not_anomaly': 0}
        if is_anomaly == 1:
            anomaly_counts[speed]['anomaly'] += 1
        else:
            anomaly_counts[speed]['not_anomaly'] += 1

    # Prepare data for plotting
    speeds = sorted(anomaly_counts.keys())
    anomaly_counts_anomaly = [anomaly_counts[speed]['anomaly'] for speed in speeds]
    anomaly_counts_not_anomaly = [anomaly_counts[speed]['not_anomaly'] for speed in speeds]

    # Plot the graph with lines
    plt.figure(figsize=(10, 6))
    plt.plot(speeds, anomaly_counts_anomaly, color='red', label='Anomaly')
    plt.plot(speeds, anomaly_counts_not_anomaly, color='blue', label='Not Anomaly')
    plt.xlabel('Speed')
    plt.ylabel('Count')
    plt.title('Anomaly vs. Not Anomaly Count for Each Speed')
    plt.legend()
    plt.grid(True)
    plt.show()
else:
    print("No anomaly data fetched.")
