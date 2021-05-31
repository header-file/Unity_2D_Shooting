using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class DataManager : MonoBehaviour
{
    public string SaveDataFileName = "SavedData.json";

    public GameData _gameData;
    public GameData GameData
    {
        get
        {
            if(_gameData == null)
            {
                LoadData();
                SaveData();
            }

            return _gameData;
        }
    }

    void Awake()
    {
        DataManager[] objs = FindObjectsOfType<DataManager>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        LoadData();
    }

    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        GameData.LoadData();        
        SaveData();
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + SaveDataFileName;

        if(File.Exists(filePath))
        {
            //Debug.Log("Load Success");

            string fromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(fromJsonData);

            //GameData.ResetData();
            
            GameData.LoadReachedStage();
        }
        else
        {
            //Debug.Log("Write New File");

            _gameData = new GameData();
        }
    }

    public void SaveData()
    {
        GameData.SaveData();
        string ToJsonData = JsonUtility.ToJson(GameData);
        string filePath = Application.persistentDataPath + SaveDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        //Debug.Log("Save Complete");
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
}
