using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private GunData _gunData;
    private GunStatData _gunStatData;
    private string savePath;

    private int _gunIdOnRespawn = -1;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, DataPlayerPrefs._pathSaveData);
        Debug.Log("Save path: " + savePath);
    }

    public void SaveDataForRespawn()
    {
        LoadDataScripTable();
        if(_gunIdOnRespawn != -1)
        {
            _gunData.gunStats[PlayerPrefs.GetInt(DataPlayerPrefs.para_IDGUN)].equip = false;
            _gunData.gunStats[_gunIdOnRespawn].equip = true;
        }    
        SaveJson();
    }    

    public void SaveData()
    {
        LoadDataScripTable();
        SaveJson();
    }

    private void LoadDataScripTable()
    {
        _gunData = LoadingData.Instance.GetGunData();
        _gunStatData = LoadingData.Instance.GetGunStatData();
    }    

    private void SaveJson()
    {
        GunProgessList gunProgessList = new GunProgessList();
        for (int i = 0; i < _gunData.gunStats.Count; i++)
        {
            var gunData = _gunData.gunStats[i];
            if (!gunData.unlock) continue;
            var gunStatData = _gunStatData.statLevels.Find(s => s.idGun == gunData.idGun);

            GunProgress gunProgress = new GunProgress
            {
                gunID = gunData.idGun,
                unlock = true,
                equip = gunData.equip,
                magSizeLevel = GetLevelUnlock(gunStatData.magSize),
                bulletSpeedLevel = GetLevelUnlock(gunStatData.bulletSpeed),
                reloadLevel = GetLevelUnlock(gunStatData.timeReload),
                fireRateLevel = GetLevelUnlock(gunStatData.fireRate),
            };
            gunProgessList.guns.Add(gunProgress);
        }

        string json = JsonUtility.ToJson(gunProgessList, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Save Done");
    }    

    private int GetLevelUnlock(List<DataLevel> dataLevels)
    {
        for(int i = dataLevels.Count - 1; i >= 0; i--)
        {
            if(dataLevels[i].unlock)
                return dataLevels[i].level;
        }
        return 1;
    }    

    public void SetGunNextSpawn(int gunID) => _gunIdOnRespawn = gunID;
   
}

[System.Serializable]
public class GunProgessList
{
    public List<GunProgress> guns = new List<GunProgress>();
}


[System.Serializable]
public class GunProgress
{
    public int gunID;
    public bool unlock;
    public bool equip;
    public int magSizeLevel;
    public int bulletSpeedLevel;
    public int reloadLevel;
    public int fireRateLevel;
}

