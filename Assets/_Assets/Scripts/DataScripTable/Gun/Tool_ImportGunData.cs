using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Tool_ImportGunData
{
    [MenuItem("Tools/Import GunData CSV")]
    public static void ImportGunData()
    {
        string filePath = Application.dataPath + "/_Assets/Data/GunBuy.csv";
        string[] lines = File.ReadAllLines(filePath);
        if(lines.Length <= 1)
        {
            Debug.Log("File Emty");
            return;
        }    

        var gunData = ScriptableObject.CreateInstance<GunData>();
        gunData.gunStats = new List<GunStat>();



        for(int i = 1; i < lines.Length; i++)
        {
            int index = 0;
            var values = lines[i].Split(',');

            var stat = new GunStat();

            stat.idGun = values[index++];
            stat.nameGun = values[index++];
            stat.priceCoin = (float)TryParseFloat(values[index++]);
            stat.priceMoney = (float)TryParseFloat(values[index++]);
            stat.unlock = (bool)TryParseBool(values[index++]);

            gunData.gunStats.Add(stat);
        }

        var folderSave = "Assets/_Assets/Scripts/DataScripTable/Gun/Data";
        Directory.CreateDirectory(folderSave);

        string assetPath = Path.Combine(folderSave, $"Gun Data.asset");

        AssetDatabase.CreateAsset(gunData, assetPath);
        AssetDatabase.SaveAssets();

    }

    static float? TryParseFloat(string str) => float.TryParse(str, out var result) ? result : null;
    static bool? TryParseBool(string str) => str.Trim().ToLower() == "true";

}
