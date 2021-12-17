using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


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
        _gameData.LoadData();
        SaveData();

        _gameData.MoveScene();
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + SaveDataFileName;
        //Debug.Log(filePath);
        if(File.Exists(filePath))
        {
            Debug.Log("Load Success");

            string fromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(fromJsonData);

            //GameData.ResetData();
            _gameData.LoadReachedStage();
        }
        else
        {
            Debug.Log("Write New File");

            _gameData = new GameData();
            _gameData.ResetData();
        }
    }

    public void SaveData()
    {
        _gameData.SaveData();
        
        string ToJsonData = JsonUtility.ToJson(_gameData);
        string filePath = Application.persistentDataPath + SaveDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        //Debug.Log("Save Complete");
    }

    public void UploadSaveData()
    {
        _gameData.SaveData();
        string ToJsonData = JsonUtility.ToJson(_gameData);
        string filePath = Application.persistentDataPath + SaveDataFileName;
        File.WriteAllText(filePath, ToJsonData);

        GameManager.Inst().Login.DBRef.Child("users").Child(_gameData.UID).Child("SaveData").SetRawJsonValueAsync(ToJsonData);

        GameManager.Inst().UiManager.MainUI.Alarm.SaveComplete();
    }

    async public void DownloadSaveData()
    {
        await GameManager.Inst().Login.DBRef.Child("users").Child(_gameData.UID).Child("SaveData").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("다운로드 실패!!!");
                return;
            }
            else if (task.IsCompleted)
            {
                _gameData = JsonUtility.FromJson<GameData>(task.Result.GetRawJsonValue());
                _gameData.LoadReachedStage();
            }
        });

        _gameData.LoadData();
        SaveData();

        GameManager.Inst().UiManager.MainUI.Alarm.LoadComplete();

        //SceneManager.LoadScene("AuthWebServer");
    }

    public void AutoSave()
    {
        SaveData();

        UploadSaveData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
}
