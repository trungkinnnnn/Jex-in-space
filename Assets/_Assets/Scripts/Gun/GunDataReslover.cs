
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GunDataReslover
{
    public static GunStat FindEquippedGun(GunData gunData)
    {
        if (gunData == null || gunData.gunStats.Count == 0) return null;
        return gunData.gunStats.FirstOrDefault(stat => stat != null && stat.equip);
    }


    public static StatLevel FindStatForGun(GunStatData gunStatData, int idGun)
    {
        if(gunStatData?.statLevels == null) return null;
        return gunStatData.statLevels.FirstOrDefault(stat => stat != null && stat.idGun == idGun);
    }

    public  static float GetHighestUnlockedValue(List<DataLevel> list)
    {
        if (list == null) return 0f;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if(list[i].unlock) return list[i].value;
        }
        return 0f;
    }

    public static GunParamasters GetParamasters(GunData gunData, GunStatData gunStatData)
    {
        var currentGun = FindEquippedGun(gunData);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_IDGUN, currentGun.idGun);
        if (currentGun == null) return new GunParamasters(null, 0, 0f, 0f, 0f);

        var statLevel = FindStatForGun(gunStatData, currentGun.idGun);
        if (statLevel == null) return new GunParamasters(currentGun, 0, 0f, 0f, 0f);

        int mag = (int)GetHighestUnlockedValue(statLevel.magSize);
        float speed = GetHighestUnlockedValue(statLevel.bulletSpeed);
        float reload = GetHighestUnlockedValue(statLevel.timeReload);
        float rate = GetHighestUnlockedValue(statLevel.fireRate);
        
        return new GunParamasters(currentGun, mag, speed, reload, rate);    
    }

}

public readonly struct GunParamasters
{
    public readonly GunStat currentGun;
    public readonly int magSize;
    public readonly float bulletSpeed;
    public readonly float timeReload;
    public readonly float fireRate;

    public GunParamasters(GunStat currentGun, int magSize, float bulletSpeed, float timeReload, float fireRate)
    {
        this.currentGun = currentGun;
        this.magSize = magSize;
        this.bulletSpeed = bulletSpeed;
        this.timeReload = timeReload;
        this.fireRate = fireRate;
    }
}
