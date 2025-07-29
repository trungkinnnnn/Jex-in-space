using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GunCotroller : MonoBehaviour
{
    private string NAME_ANI_TRIGGER_SHOOT = "isShoot";
    private Animator _animator;
    [SerializeField] Transform pointFire;

    //data
    [SerializeField] GunData _gunData;
    [SerializeField] GunStatData _gunStatData;

    // Name paramaster
    private string NAME_MAG_SIZE = "magSize";
    private string NAME_BULLET_SPEED = "bulletSpeed";
    private string NAME_TIME_RELOAD = "timeReload";
    private string NAME_FIRE_RATE = "fireRate";

    private float magSize;
    private float bulletSpeed;
    private float timeReload;
    private float fireRate;



    private GunStat currentGun;
    private List<StatLevel> _levelList;

    private float currentSpeedBullet;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _levelList = new List<StatLevel>();
        SetGunEquip();
        SetGunStatEquip();
        SetParamasterStat();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger(NAME_ANI_TRIGGER_SHOOT);
            FireBullet();
        }    
    }


    private void FireBullet()
    {
        GameObject bullet = Instantiate(currentGun.bulletPrefabs, pointFire.position, pointFire.rotation);
        BulletController bulletController = bullet.GetComponent<BulletController>();    
        if(bulletController != null ) bulletController.Init(pointFire.right, bulletSpeed);
    }


    private void SetGunEquip()
    {
        foreach(var gun in _gunData.gunStats)
        {
            if(gun.unlock && gun.equip)
            {
                currentGun = gun;
                break;
            }
        }
    }

    private void SetGunStatEquip()
    {
        bool finded = false;    
        foreach(var gunStat in _gunStatData.statLevels)
        {
            if (gunStat.idGun != currentGun.idGun)
            {
                if (finded) break;
                continue;
            }
            else
            {
                _levelList.Add(gunStat);
                finded = true;
            }

        }
    }

    private void SetParamasterStat()
    {
        foreach (var stat in _levelList)
        {
            if (stat.nameStat == NAME_MAG_SIZE && stat.stats.unlock)
            {
                magSize = stat.stats.value;
                continue;
            }
            if (stat.nameStat == NAME_BULLET_SPEED && stat.stats.unlock)
            {
                bulletSpeed = stat.stats.value;
                continue;
            }
            if (stat.nameStat == NAME_TIME_RELOAD && stat.stats.unlock)
            {
                timeReload = stat.stats.value;
                continue;
            }
            if (stat.nameStat == NAME_FIRE_RATE && stat.stats.unlock)
            {
                fireRate = stat.stats.value;
                continue ;
            }
        }
        Debug.Log($"{magSize} + {bulletSpeed} + {timeReload} + {fireRate}");
    }


}
