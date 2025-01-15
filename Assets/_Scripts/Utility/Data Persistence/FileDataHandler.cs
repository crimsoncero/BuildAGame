using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
    private string _dataDirPath = string.Empty;
    private string _dataFileName = string.Empty;
    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord;

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption, string encryptionCodeWord)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _useEncryption = useEncryption;
        _encryptionCodeWord = encryptionCodeWord;
    }

    public GameData Load()
    {
        // using Path.Combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = string.Empty;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Optionally decrypt data
                dataToLoad = EncryptDecrypt(dataToLoad); 

                // Deserialize the data from Json back into the C# object 
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // using Path.Combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            // Create the directory the file will be written to if it doesn't already exist.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into Json.
            string dataToStore = JsonUtility.ToJson(data, true);

            // Encrypt data
            if(_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }

    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
