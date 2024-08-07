using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
	string _filePath;

	void Awake()
	{
		_filePath = Path.Combine(Application.persistentDataPath, "savefile.dat");
	}

	public void SaveData(SaveData data)
	{
		Debug.Log(data.IntValue);
		BinaryFormatter formatter = new BinaryFormatter();
		try
		{
			using (FileStream fileStream = new FileStream(_filePath, FileMode.Create))
			{
				formatter.Serialize(fileStream, data);
			}
		}
		catch (IOException e)
		{
			Debug.LogError("An error occurred while saving the data: " + e.Message);
		}
	}

	public SaveData LoadData()
	{
		if (File.Exists(_filePath))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				using (FileStream fileStream = new FileStream(_filePath, FileMode.Open))
				{
					SaveData data = (SaveData)formatter.Deserialize(fileStream);
					return data;
				}
			}
			catch (IOException e)
			{
				return null;
			}
		}
		else
		{
			return new SaveData();
		}
	}
}