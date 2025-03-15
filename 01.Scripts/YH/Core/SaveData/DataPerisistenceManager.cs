using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPerisistenceManager : MonoSingleton<DataPerisistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData _gameData;
    private List<IDataPerisistence> dataPerisistenceObjects;
    private FileHandler dataHandler;

    private void Start()
    {
        this.dataHandler = new FileHandler(Application.persistentDataPath,fileName);
        this.dataPerisistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void NewGame()
    {
        this._gameData = new GameData();
        foreach(IDataPerisistence dataPerisistence in dataPerisistenceObjects)
        {
            dataPerisistence.LoadData(_gameData);
        }
    }

    public void LoadGame()
    {
        this._gameData = dataHandler.Load();
        if (this._gameData == null)
        {
            Debug.Log("No data was found. initializing data to default");
            NewGame();
        }
        foreach(IDataPerisistence dataPerisitenceObj in dataPerisistenceObjects)
        {
            dataPerisitenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach(IDataPerisistence dataPerisistence in dataPerisistenceObjects)
        {
            dataPerisistence.SaveData(ref _gameData);
        }

        dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<IDataPerisistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPerisistence> dataPerisistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPerisistence>();
        return new List<IDataPerisistence>(dataPerisistenceObjects);
    }
}
