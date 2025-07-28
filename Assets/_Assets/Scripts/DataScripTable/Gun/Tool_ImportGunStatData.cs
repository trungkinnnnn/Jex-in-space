using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

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

        for (int i = 1; i < lines.Length; i++)
        {
            int index = 0;
            var values = lines[i].Trim().Split(',');
  
            var statLevel = new StatLevel();
         
            statLevel.idGun = values[index++];
            statLevel.nameGun = values[index++];

            statLevel.stats = new DataLevel();
            statLevel.stats.level = (int)TryParseInt(values[index++]);
            statLevel.stats.value = (float)TryParseFloat(values[index++]);
            statLevel.stats.price = (float)TryParseFloat(values[index++]);
            statLevel.stats.unlock = (bool)TryParseBool(values[index++]);
        
               
            gunStatData.statLevels.Add(statLevel);

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
 }
