using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Reader : MonoBehaviour
{
    public SavedLevel read(string file)
    {
        const string path = "Levels/";
        string jsonLevel;
        try
        {
            jsonLevel = Resources.Load<TextAsset>(path + file).text;
        }
        catch (System.Exception)
        {
            throw;
        }

		try
		{
            SavedLevel level = JsonUtility.FromJson<SavedLevel>(jsonLevel);
            return level;
		}
		catch (System.Exception)
		{
			throw;
		}
	}


   

    void Start(){}
    void Update(){}
}
