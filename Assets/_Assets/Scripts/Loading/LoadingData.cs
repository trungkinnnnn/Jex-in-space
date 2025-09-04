
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LoadingData : MonoBehaviour
{
    public static Action OnLoadData;

    [SerializeField] GunData _gunData;
    [SerializeField] GunStatData _gunStatData;
    private GunData _gunDataClone;
    private GunStatData _gunStatDataClone;
    private List<bool> _listSettings = new();

    private string _pathData;

    public static LoadingData Instance;
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
        LoadDataGun();
        LoadingDataSetting();
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

    private void LoadingDataSetting()
    {
        string setting = PlayerPrefs.GetString(DataPlayerPrefs.para_Setting, "");
        if (setting == "")
        {
            setting = "true,true,true";
        }

        var listSetting = setting.Split(',');

        for (int i = 0; i < listSetting.Length; i++)
        {  
            _listSettings.Add((bool)TryPaserBool(listSetting[i]));
        }    
    }    
    private bool? TryPaserBool(string str) => str.Trim().ToLower() == "true";

    public bool ActiveSoundFX() => _listSettings[0];
    public bool ActiveSoundMusic() => _listSettings[1];
    public bool ActiveCameraShake() => _listSettings[2];

    public GunData GetGunData() => _gunDataClone;
    public GunStatData GetGunStatData() => _gunStatDataClone;

}
