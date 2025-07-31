using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class Tool_ImportGunStatData
{
    [MenuItem("Tools/ Import GunStatData From CSV")]
    public static void ImportGunStatData()
    {
        string fileAssets = Application.dataPath + "/_Assets/Data/GunUpdate.csv";
        string[] lines = File.ReadAllLines(fileAssets);
        if (lines.Length <= 1)
        {
            Debug.Log("File Empty");
            return;
        }

        var gunStatData = ScriptableObject.CreateInstance<GunStatData>();
        gunStatData.statLevels = new List<StatLevel>();

        var currentStatLevel = new StatLevel();
        currentStatLevel.magSize = new List<DataLevel>();
        currentStatLevel.bulletSpeed = new List<DataLevel>();
        currentStatLevel.timeReload = new List<DataLevel>();
        currentStatLevel.fireRate = new List<DataLevel>();

        string currentId = "";

        for (int i = 1; i < lines.Length; i++)
        {
            int index = 0;
            var values = lines[i].Trim().Split(',');

            if(currentId == values[0] || currentId == "")
            {
                // 
            }else
            {
                gunStatData.statLevels.Add(currentStatLevel);
                currentStatLevel = new StatLevel();
                currentStatLevel.magSize = new List<DataLevel>();
                currentStatLevel.bulletSpeed = new List<DataLevel>();
                currentStatLevel.timeReload = new List<DataLevel>();
                currentStatLevel.fireRate = new List<DataLevel>();
            }

            currentId = values[0];
            currentStatLevel.idGun = values[index++];

            if (values[1] == "magSize")
            {
                DataLevel value = data(values, index);
                currentStatLevel.magSize.Add(value);
            }

            if (values[1] == "bulletSpeed")
            {
                DataLevel value = data(values, index);
                currentStatLevel.bulletSpeed.Add(value);
            }

            if (values[1] == "timeReload")
            {
                DataLevel value = data(values, index);
                currentStatLevel.timeReload.Add(value);
            }

            if (values[1] == "fireRate")
            {
                DataLevel value = data(values, index);
                currentStatLevel.fireRate.Add(value);
            }

          

        }

        string filePath = "Assets/_Assets/Scripts/DataScripTable/Gun/Data";
        Directory.CreateDirectory(filePath);

        string assetFile = Path.Combine(filePath, $"Gun Stat Data.asset");
        AssetDatabase.CreateAsset(gunStatData, assetFile);
        AssetDatabase.SaveAssets();

    }

    static int? TryParseInt(string str) => int.TryParse(str, out var value) ? value : null;
    static float? TryParseFloat(string str) => float.TryParse(str, out var value) ? value : null;

    static bool? TryParseBool(string str) => str.Trim().ToLower() == "true";

    private static DataLevel data(string[] values, int index)
    {
        var value = new DataLevel();
        value.name = values[index++];
        value.level = (int)TryParseInt(values[index++]);
        value.value = (float)TryParseFloat(values[index++]);
        value.price = (float)TryParseInt(values[index++]);
        value.unlock = (bool)TryParseBool(values[index++]);
        return value;
    }

}
