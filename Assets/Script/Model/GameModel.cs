using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameModel 
{
    public List<LevelData> GenerateAllLevels()
    {
        List<LevelData> allLevels = new List<LevelData>();

        for (int i = 1; i <= 10; i++)
        {
            LevelData levelData = GenerateLevel(i);
            allLevels.Add(levelData);
        }



        return allLevels;
    }

    private LevelData GenerateLevel(int levelNumber)
    {
        LevelData data = new LevelData();
        data.Level = levelNumber;
        data.playerInfo = new PlayerInfo();
        data.enemyInfo = new EnemyInfo();
        data.allyInfo = new AllyInfo();

        int levelType = levelNumber % 10;
        if (levelNumber == 10) levelType = 10;


        if (levelType >= 1 && levelType <= 3)
        {
            data.mode = "1 vs 1";
            data.playerCount = 1;
            data.allyCount = 0;
            data.enemyCount = 1;
        }
        else if (levelType >= 4 && levelType <= 6)
        {
            data.mode = "1 vs n";
            data.playerCount = 1;
            data.allyCount = 0;
            data.enemyCount = levelNumber - 2;
        }
        else if (levelType >= 7 && levelType <= 10)
        {
            data.mode = "n vs n";
            data.playerCount = 1;
            data.allyCount = levelNumber - 6;
            data.enemyCount = (levelNumber - 6) * 2 + 3; 
        }



        float enemyBaseBonus = (levelNumber - 1) * 0.11f;
        data.enemyInfo.enemy_health_bonus = enemyBaseBonus;
        data.enemyInfo.enemy_damage_bonus = enemyBaseBonus;

        float enemySpeedBonus = (levelNumber - 1) * 0.04f;
        data.enemyInfo.enemy_speed_bonus = enemySpeedBonus;


        int playerTeamSize = data.playerCount + data.allyCount;
        float teamDifference = (float)(data.enemyCount - playerTeamSize);

        if (teamDifference > 0) 
        {
            float balanceBonus = (teamDifference / 2.0f) * 0.05f;

            data.playerInfo.player_health_bonus += balanceBonus;
            data.allyInfo.ally_health_bonus += balanceBonus;

            data.playerInfo.player_damage_bonus += balanceBonus * 0.3f;
            data.allyInfo.ally_damage_bonus += balanceBonus * 0.3f;
            data.playerInfo.player_speed_bonus += balanceBonus * 3f;
            data.allyInfo.ally_speed_bonus += balanceBonus * 0.3f;
            data.playerInfo.cooldown_attack_bonus = -balanceBonus * 0.2f;
        }
        else if (teamDifference < 0) 
        {
            float balanceBonus = (Mathf.Abs(teamDifference) / 2.0f) * 0.05f;
            data.enemyInfo.enemy_health_bonus += balanceBonus;
            data.enemyInfo.enemy_speed_bonus += balanceBonus * 0.2f;
        }

        int cooldownTier = levelNumber / 3;
        data.enemyInfo.cooldown_attack_bonus = -cooldownTier * 0.1f;

        int totalUnits = playerTeamSize + data.enemyCount;
        data.maxSizeCountRow = Mathf.Clamp(totalUnits / 2, 3, 10);

        return data;
    }

    public void GenerateAndSaveLevels()
    {
        List<LevelData> generatedLevels = GenerateAllLevels();

        SaveDataObject saveData = new SaveDataObject();
        saveData.Levels = generatedLevels;

        string json = JsonConvert.SerializeObject(saveData, Newtonsoft.Json.Formatting.Indented);


        bool success = WriteData(json, CONST.SAVE_FILE_NAME); 

        if (success)
        {
            Debug.Log("Success file levels.json!");
        }
    }


    public LevelData GenerateChallengeLevel()
    {
        LevelData data = new LevelData();
        data.Level = 11; 
        data.mode = "n vs n";
        data.playerCount = 1;
        data.allyCount = 24;
        data.enemyCount = 25;

        data.playerInfo = new PlayerInfo();
        data.enemyInfo = new EnemyInfo();
        data.allyInfo = new AllyInfo();

 

        data.maxSizeCountRow = 10; 

        return data;
    }

    public void SaveLevels(int level)
    {

        string json = JsonConvert.SerializeObject(level, Newtonsoft.Json.Formatting.Indented);
        bool success = WriteData(json, CONST.SAVE_FILE_LEVEL);

        if (success)
        {
            Debug.Log("Success file levels.json!");
        }
    }
    public int LoadLevels()
    {
        if (int.TryParse(ReadData(CONST.SAVE_FILE_LEVEL), out int level))
        {
            return level;
        }
        return 0;
    }

    public bool WriteData(string content, string fileName)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            File.WriteAllText(filePath, content);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving data to {fileName}: {ex.Message}");
            return false;
        }
    }

    public string ReadData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error loading data from {fileName}: {ex.Message}");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    [System.Serializable]
    public class SaveDataObject
    {
        public List<LevelData> Levels;
    }
    public List<LevelData> LoadAllLevels()
    {
        string json = ReadData(CONST.SAVE_FILE_NAME);

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("Save file is empty or not found. Cannot load levels.");
            return new List<LevelData>();
        }

        try
        {
            SaveDataObject loadedSaveData = JsonConvert.DeserializeObject<SaveDataObject>(json);
            return loadedSaveData.Levels;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to deserialize level data: {ex.Message}");
            return new List<LevelData>();
        }
    }



    public int CheckLevel(int level)
    {
        level = level + 1;
        if (level > 9)
        {
            level = 9;
        }
        return level;
    }
}
