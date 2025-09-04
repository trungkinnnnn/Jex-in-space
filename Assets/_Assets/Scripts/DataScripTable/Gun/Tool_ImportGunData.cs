
#if UNITY_EDITOR
using System.Collections.Generic;

using System.IO;
using UnityEditor; // Thêm dòng này
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

            stat.idGun = (int)TryParseInt(values[index++]);
            stat.nameGun = values[index++];
            stat.priceCoin = (int)TryParseInt(values[index++]);
            stat.priceMoney = (int)TryParseInt(values[index++]);
            stat.unlock = (bool)TryParseBool(values[index++]);

            gunData.gunStats.Add(stat);
        }

        var folderSave = "Assets/_Assets/Scripts/DataScripTable/Gun/Data";
        Directory.CreateDirectory(folderSave);

        string assetPath = Path.Combine(folderSave, $"Gun Data.asset");

        AssetDatabase.CreateAsset(gunData, assetPath);
        AssetDatabase.SaveAssets();

    }
    
    static int? TryParseInt(string str) => int.TryParse(str, out var result) ? result : null;

    static float? TryParseFloat(string str) => float.TryParse(str, out var result) ? result : null;
    static bool? TryParseBool(string str) => str.Trim().ToLower() == "true";

}

#endif