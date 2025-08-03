using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGetData
{
    private GunData _gunData;
    private GunStatData _gunStatData;

    private float magSize;
    private float bulletSpeed;
    private float timeReload;
    private float fireRate;

    private StatLevel _levelList = new StatLevel();
    private GunStat currentGun;

    public GunGetData(GunData gunData, GunStatData gunStatData)
    {
        _gunData = gunData;
        _gunStatData = gunStatData;
    }

    public void StartTakeData()
    {
        SetGunEquip();
        SetGunStatEquip();
        SetParamasterStat();
    }

    public float GetMagSize() => magSize;
    public float GetBulletSpeed() => bulletSpeed;
    public float GetTimeReload() => timeReload;
    public float GetFireRate() => fireRate;
    public GunStat CurrentGun() => currentGun;
    
    // lấy súng eqiup
    private void SetGunEquip()
    {
        foreach (var gun in _gunData.gunStats)
        {
            if (gun.unlock && gun.equip)
            {
                currentGun = gun;
                break;
            }
        }
    }

    // lấy thông tin khẩu súng đang sử dụng
    private void SetGunStatEquip()
    {
        bool finded = false;
        foreach (var gunStat in _gunStatData.statLevels)
        {
            if (gunStat.idGun != currentGun.idGun)
            {
                if (finded) break;
                continue;
            }
            else
            {
                _levelList = gunStat;
                finded = true;
            }

        }
    }
    
    // lấy thông số của súng
    private void SetParamasterStat()
    {
        magSize = TakeValueList(_levelList.magSize);
        bulletSpeed = TakeValueList(_levelList.bulletSpeed);
        timeReload = TakeValueList(_levelList.timeReload);
        fireRate = TakeValueList(_levelList.fireRate);
        Debug.Log($"{magSize} + {bulletSpeed} + {timeReload} + {fireRate}");
    }    

    private float TakeValueList(List<DataLevel> data)
    {
        foreach (var value in data)
        {
            if(value.unlock)
            {
                return value.value;
            }    
        }
        return 0f;
    }

}
