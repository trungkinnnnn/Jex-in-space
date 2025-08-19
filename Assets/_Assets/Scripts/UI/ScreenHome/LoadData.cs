
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    public static Action OnLoadData;

    [SerializeField] GunData _gunData;
    [SerializeField] GunStatData _gunStatData;


    private GunData _gunDataClone;
    private GunStatData _gunStatDataClone;
    private static string _para_NAMESCENE_NEXT = "InGameScreen";
    private string _pathData;

    public static LoadData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            
        }

        _pathData = Path.Combine(Application.persistentDataPath, DataPlayerPrefs._pathSaveData);
        Debug.Log("Load path: " + _pathData);
        _gunDataClone = Instantiate(_gunData);
        _gunStatDataClone = Instantiate(_gunStatData);
    }

    private void Start()
    {
        LoadDataGun();
    }

    private void LoadDataGun()
    {
        if(!File.Exists(_pathData))
        {
            Debug.Log("File null");
            return;
        }

        string json = File.ReadAllText(_pathData);
        GunProgessList loads = JsonUtility.FromJson<GunProgessList>(json);

        foreach(var gun in loads.guns)
        {
            GunStat gunStat = _gunDataClone.gunStats.Find(s => s.idGun == gun.gunID);
            StatLevel levels = _gunStatDataClone.statLevels.Find(s => s.idGun == gun.gunID);
            SetGunStat(gunStat, gun);
            SetGunLevel(levels, gun);
        }

        OnLoadData?.Invoke();
    }    

    private void SetGunStat(GunStat gunStat, GunProgress gunProgress)
    {
        gunStat.equip = gunProgress.equip;
        gunStat.unlock = gunProgress.unlock;
    }

    private void SetGunLevel(StatLevel level, GunProgress levelProgress)
    {
        level.magSize[levelProgress.magSizeLevel - 1].unlock = true;
        level.bulletSpeed[levelProgress.bulletSpeedLevel - 1].unlock = true;
        level.timeReload[levelProgress.reloadLevel - 1].unlock = true;
        level.fireRate[levelProgress.fireRateLevel - 1].unlock = true;
    }

    public void ActionNextScene()
    {
        SceneManager.LoadScene(_para_NAMESCENE_NEXT);
    }

    public GunData GetGunData() => _gunDataClone;

    public GunStatData GetGunStatData() => _gunStatDataClone; 

    public static void LogGunStatData(GunStatData gunStatData)
    {
        if (gunStatData == null || gunStatData.statLevels == null)
        {
            Debug.Log("GunStatData hoặc statLevels null!");
            return;
        }

        foreach (var statLevel in gunStatData.statLevels)
        {
            Debug.Log($"=== Gun ID: {statLevel.idGun} ===");

            LogDataLevelList("Mag Size", statLevel.magSize);
            LogDataLevelList("Bullet Speed", statLevel.bulletSpeed);
            LogDataLevelList("Time Reload", statLevel.timeReload);
            LogDataLevelList("Fire Rate", statLevel.fireRate);
        }
    }

    private static void LogDataLevelList(string statName, List<DataLevel> levels)
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.Log($"{statName}: Không có dữ liệu");
            return;
        }

        Debug.Log($"{statName}:");

        foreach (var data in levels)
        {
            Debug.Log($"  Name: {data.name}, Level: {data.level}, Value: {data.value}, Price: {data.price}, Unlock: {data.unlock}");
        }
    }

    public void LogGunData(GunData gunData)
    {
        if (gunData == null)
        {
            Debug.Log("GunData is NULL!");
            return;
        }

        if (gunData.gunStats == null || gunData.gunStats.Count == 0)
        {
            Debug.Log("GunData.gunStats is EMPTY!");
            return;
        }

        foreach (var stat in gunData.gunStats)
        {
            Debug.Log(
                $"ID: {stat.idGun} | " +
                $"Name: {stat.nameGun} | " +
                $"Coin: {stat.priceCoin} | " +
                $"Money: {stat.priceMoney} | " +
                $"Unlock: {stat.unlock} | " +
                $"Equip: {stat.equip} | " +
                $"GunPrefab: {(stat.gunPrefabs ? stat.gunPrefabs.name : "null")} | " +
                $"BulletPrefab: {(stat.bulletPrefabs ? stat.bulletPrefabs.name : "null")}"
            );
        }
    }



}
