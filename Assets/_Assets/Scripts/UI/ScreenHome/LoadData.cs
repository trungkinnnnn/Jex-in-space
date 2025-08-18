
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{

    [SerializeField] GunData _gunData;
    [SerializeField] GunStatData _gunStatData;

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
            GunStat gunStat = _gunData.gunStats.Find(s => s.idGun == gun.gunID);
            StatLevel levels = _gunStatData.statLevels.Find(s => s.idGun == gun.gunID);
            SetGunStat(gunStat, gun);
            SetGunLevel(levels, gun);
        }

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

    public GunData GetGunData() => _gunData;
    public GunStatData GetGunStatData() => _gunStatData;




}
