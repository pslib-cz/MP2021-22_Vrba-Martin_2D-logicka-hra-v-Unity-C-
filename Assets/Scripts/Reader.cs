using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//[RequireComponent(typeof(SavedLevel))]
public class Reader : MonoBehaviour
{
    private SavedLevel level;
    private string savePath;
    const string ext = ".txt";

    // Start is called before the first frame update
    void Start()
    {
        //level =  ...

        savePath = Application.dataPath + "/Levels/";

        //read("level1");

        SavedLevel testt = new SavedLevel()
        {
            Name = "testt",
            Entities = new EntityConstructor[3]
        };

        testt.Entities[0] = new EntityConstructor() { T = TileType.Wall, CoordinateX = 1, CoordinateY = 0 };
        testt.Entities[1] = new EntityConstructor() { T = TileType.Floor, CoordinateX = 1, CoordinateY = 1 };
        testt.Entities[2] = new EntityConstructor() { T = TileType.Player, CoordinateX = 1, CoordinateY = 2, Direction = Direction.Up };

        save(testt);
    }

    //public SavedLevel ReadLevel(string name)
    public SavedLevel read(string file){
        StreamReader reader = new StreamReader(savePath + file + ext);
        string jsonLevel = reader.ReadToEnd();
        //Debug.Log(jsonLevel);
        //Debug.Log(jsonLevel);
		try
		{
            SavedLevel level = JsonUtility.FromJson<SavedLevel>(jsonLevel);
            return level;
            //Debug.Log(level);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

    public void save(SavedLevel level)
	{
        StreamWriter writer = new StreamWriter(savePath + level.Name + ext);
        string jsonLevel = JsonUtility.ToJson(level);
        //Debug.Log(jsonLevel);
        writer.Write(jsonLevel);
        writer.Close();
	}
   


    // Update is called once per frame
    void Update()
    {
        
    }
}
