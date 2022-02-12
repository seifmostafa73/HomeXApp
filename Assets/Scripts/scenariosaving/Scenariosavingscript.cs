using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Scenariosavingscript
{
    public enum Actions { OFF = 0, ON, TOGGLE };
    public static string directory = "/SavedJason";
    public static string FileName = "Scenarios.txt";

    [System.Serializable] public struct DATA
    {
        public string ScenarioName;
        public int icon;
        public int[] Function;//holds Action ->ON=1, OFF=0, TOGGLE=2  , and channel and device [ext ON-1-0 == open second device in product number 0]
        public string[] Channel;
        public bool HomeEnterTrigger;
        public bool HomeExitTrigger;
        
        public DATA(string SName, int Sicon, int[]Fnc, string[]Chn,bool HomeTrigger,bool ExitTrigger) 
        {
            ScenarioName = SName;
            icon = Sicon;
            Function = Fnc;
            Channel = Chn;
            HomeEnterTrigger = HomeTrigger;
            HomeExitTrigger = ExitTrigger;
        }
    };

    public static void Save_Scenario_inJASON(List<DATA> Object)
    {
        string direct = Application.persistentDataPath + directory;
        if (!Directory.Exists(direct)) { Directory.CreateDirectory(direct); }
        string JSONDATA = null;
        for (int i = 0; i < Object.Count; i++)
        {
            if (i+1 != Object.Count)
            {
                JSONDATA += JsonUtility.ToJson(Object[i]) + ",";
            }
            else
            {
                JSONDATA += JsonUtility.ToJson(Object[i]);
            }
        }
        Debug.Log(JSONDATA);
        File.WriteAllText(direct + FileName, "["+JSONDATA+"]");
        PlayerPrefs.Save();
    }

    public static List<DATA> Load_Scenario_Data()
    {
        string path = Application.persistentDataPath + directory + FileName;
        List<DATA> SavedObject;
        if (File.Exists(path))
        {
            string jasonData = File.ReadAllText(path);
            Debug.Log(jasonData);
            var SavedArray = JsonHelper.GetArray<DATA>(jasonData);
            SavedObject = new List<DATA>(SavedArray);
            Debug.Log(SavedObject);
            return SavedObject;
        }
        else
        {
            Debug.LogError("JSON FILE WAS NOT CREATED");
            Debug.LogError(path);
            File.WriteAllText(path, "{}");
            Load_Scenario_Data();
            return null;
        }

    }

}
