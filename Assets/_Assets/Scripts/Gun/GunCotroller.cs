using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GunGetData))]
public class GunCotroller : MonoBehaviour
{
    [SerializeField] GunData data;
    [SerializeField] GunStatData stat;
    [SerializeField] Transform pointFire;
    private GunGetData _gunGetData;
    private Animator _animator;

    //Data
    private GunStat currentGun;
    private float magSize;
    private float bulletSpeed;
    private float timeReload;
    private float fireRate;
    private string idGun;

    //
    private float currentMagSizebullet;
    private float totalbullet;

    //IDBULLET
    private readonly string ID_BULLET_REDPLASMA = "Gun05";
    public float angleRedPlasma = 30f;

    //Ani
    private string NAME_ANI_TRIGGER_SHOOT = "isShoot";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gunGetData = new GunGetData(data, stat);
        _gunGetData.StartTakeData();
        SetDataGun();

        totalbullet = magSize * 3;
        currentMagSizebullet = magSize;
        Debug.Log("Total : " + totalbullet);
    }

    private void Update()
    {
        if (InputManager.isInputLocked) return;

        if (Input.GetMouseButtonDown(0) && currentMagSizebullet > 0 && FireRate.canShoot)
        {
            _animator.SetTrigger(NAME_ANI_TRIGGER_SHOOT);
            StartCoroutine(FireBullet());
        }
    }

    private IEnumerator FireBullet()
    {
        FireRate.canShoot = false;

        currentMagSizebullet -= 1;

        GameObject bullet = Instantiate(currentGun.bulletPrefabs, pointFire.position, pointFire.rotation);
        BulletBase bulletController = bullet.GetComponent<BulletBase>();
        if (bulletController != null) bulletController.Init(pointFire.right, bulletSpeed);

        if(currentGun.idGun == ID_BULLET_REDPLASMA)
        {
            HandleBulletRedPlasma();
        }    

        if (currentMagSizebullet == 0)
        {
            Reload();
        }
        else
        {
            yield return new WaitForSeconds(fireRate);
            FireRate.canShoot = true;
        }
    }

    private void Reload()
    {
        StartCoroutine(LockInputForSecons(timeReload));
        Debug.Log("current totalAmor : " + totalbullet);
    }

    private IEnumerator LockInputForSecons(float timeReload)
    {
        InputManager.isInputLocked = true;
        yield return new WaitForSeconds(timeReload);
        InputManager.isInputLocked = false;
        if (magSize <= totalbullet)
        {
            currentMagSizebullet = magSize;
            totalbullet -= currentMagSizebullet;
        }
        else
        {
            currentMagSizebullet = totalbullet;
        }
        FireRate.canShoot = true;

        if (currentMagSizebullet == 0)
        {
            Debug.Log("Total : " + totalbullet + "Het Dan");
            InputManager.isInputLocked = true;
        }
    }

    private void SetDataGun()
    {
        currentGun = _gunGetData.CurrentGun();
        magSize = _gunGetData.GetMagSize();
        bulletSpeed = _gunGetData.GetBulletSpeed();
        timeReload = _gunGetData.GetTimeReload();
        fireRate = _gunGetData.GetFireRate();
    }


    private void OnEnable()
    {
        BoxAmor.OnBoxBroken += HandleTakeAmor;
    }

    private void OnDisable()
    {
        BoxAmor.OnBoxBroken -= HandleTakeAmor;
    }    

    private void HandleTakeAmor(int amor)
    {
        totalbullet += amor;
        Debug.Log("Amor last : " + totalbullet);
    }    

    private void HandleBulletRedPlasma()
    {
        float[] angles = {angleRedPlasma, - angleRedPlasma};
        foreach(var angle  in angles)
        {
            Vector3 dir = Quaternion.Euler(0f , 0f, angle) * pointFire.right;
            GameObject bullet = Instantiate(currentGun.bulletPrefabs, pointFire.position, Quaternion.Euler(0f, 0f, angle) * pointFire.rotation);
            BulletBase bulletController = bullet.GetComponent<BulletBase>();
            if (bulletController != null) bulletController.Init(dir, bulletSpeed);
        }    
    }    

}
