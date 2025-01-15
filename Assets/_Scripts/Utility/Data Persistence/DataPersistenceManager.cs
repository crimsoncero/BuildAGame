using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName = "Data.Game";
    [SerializeField] private bool _useEncryption = false;
    [SerializeField] private string _encryptionCodeWord = "CodeWord";
    public static DataPersistenceManager Instance { get; private set; }
    
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;
    
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
        }

        Instance = this;
    }

    private void Start()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption, _encryptionCodeWord);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    

    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        _gameData = _dataHandler.Load();

        // if no data can be loaded, intialize to a new game
        if(_gameData == null)
        {
            Debug.Log("No data was found. Initializying data to defaults");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }

    }
    public void SaveGame()
    {
        //  Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }

        // Save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
