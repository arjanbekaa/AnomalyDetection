using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class PlayerData
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public float Time { get; set; }
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public float PlayerSpeed { get; set; }
}

public class DataManager : MonoSingleton<DataManager>
{
    private string dataFilePath = "Assets\\_Scripts\\DataFiles\\player_data.txt";
    StreamWriter fileWriter;
    
    private string anomalyDataFilePath = "Assets\\_Scripts\\DataFiles\\player_anomaly_data.txt";
    StreamWriter anomalyFileWriter;

    public override void Init()
    {
        persistOnSceneLoad = false;
        base.Init();
    }

    public void SavePlayerData(PlayerData playerData)
    {
        try
        {
            // Create or initialize the StreamWriter
            if (fileWriter == null)
            {
                fileWriter = new StreamWriter(dataFilePath, true);
            }

            // Append player data to the file
            fileWriter.WriteLine($"{playerData.PlayerId},{playerData.Time},{playerData.PlayerX},{playerData.PlayerY},{playerData.PlayerSpeed}");
            fileWriter.Flush(); // Flush to ensure data is written immediately
            Debug.Log("Player data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    public void UpdatePlayerData(int playerId, string playerName, float time, float playerX, float playerY, float playerSpeed)
    {
        PlayerData currentPlayerData = new PlayerData
        {
            PlayerId = playerId,
            PlayerName = playerName,
            Time = time,
            PlayerX = playerX,
            PlayerY = playerY,
            PlayerSpeed = playerSpeed
        };

        // Save updated player data
        SavePlayerData(currentPlayerData);
    }

    public void AddPlayerAnomalyData(PlayerAnomalyData anomalyData)
    {
        SavePlayerAnomalyData(anomalyData);
    }

    private void SavePlayerAnomalyData(PlayerAnomalyData anomalyData)
    {
        try
        {
            // Create or initialize the StreamWriter
            if (anomalyFileWriter == null)
            {
                anomalyFileWriter = new StreamWriter(anomalyDataFilePath, true);
            }

            // Append anomaly data to the file
            anomalyFileWriter.WriteLine($"{anomalyData.playerSpeed},{anomalyData.isAnomaly}");
            anomalyFileWriter.Flush(); // Flush to ensure data is written immediately
            Debug.Log("Player anomaly data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save player anomaly data: {e.Message}");
        }
    }
    
    public List<PlayerAnomalyData> GetPlayerAnomalyData()
    {
        List<PlayerAnomalyData> anomalyData = new List<PlayerAnomalyData>();

        if (File.Exists(anomalyDataFilePath))
        {
            using (StreamReader reader = new StreamReader(anomalyDataFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    PlayerAnomalyData data = new PlayerAnomalyData
                    {
                        playerSpeed = float.Parse(parts[0]),
                        isAnomaly = int.Parse(parts[1])
                    };
                    anomalyData.Add(data);
                }
            }
        }

        return anomalyData;
    }
    
    public List<PlayerData> LoadPlayerData()
    {
        List<PlayerData> playerData = new List<PlayerData>();

        if (File.Exists(dataFilePath))
        {
            using (StreamReader reader = new StreamReader(dataFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    PlayerData data = new PlayerData
                    {
                        PlayerId = int.Parse(parts[0]),
                        Time = float.Parse(parts[1]),
                        PlayerX = float.Parse(parts[2]),
                        PlayerY = float.Parse(parts[3]),
                        PlayerSpeed = float.Parse(parts[4])
                    };
                    playerData.Add(data);
                }
            }
        }

        return playerData;
    }

    public List<float> GetPlayerSpeedData(int playerId)
    {
        List<PlayerData> playerData = LoadPlayerData();
        List<float> playerSpeeds = new List<float>();

        foreach (var data in playerData)
        {
            if (data.PlayerId == playerId)
            {
                playerSpeeds.Add(data.PlayerSpeed);
            }
        }

        return playerSpeeds;
    }
}

[System.Serializable]
public class PlayerAnomalyData
{
    public float playerSpeed;
    // if isAnomaly == 1 ? is Anomaly else is Not Anomaly
    public int isAnomaly;
}