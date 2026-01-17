using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    // HUD controller
    public static Action<int> OnActionCurrentBullet;
    public static Action<int> OnActionTotalBullet;
    public static Action<float> OnActionReloading;

    //DieScreenUI
    public static Action Die;

    [Header("PointFire")]
    [SerializeField] Transform _pointFire;

    [Header("Trash")]        
    [SerializeField] Transform _positionGun;
    [SerializeField] Transform _positionSpawnCasing;
    [SerializeField] Transform _positionSpawnMagazine;
    [SerializeField] GameObject _prefabCasing;
    [SerializeField] GameObject _prefabMagazine;

    [Header("Audio")]
    [SerializeField] AudioData _audioSFXDataShoting;
    [SerializeField] AudioData _audioShellCas;
    private AudioSource _audioSource;

    public float _upVolume = 1.5f;

    // Data
    private GunData _gunData;
    private GunStatData _gunStatData;
  
    // runtime
    private Animator _animator;
    private Transform _pointFireTf;
    private GunParamasters _paramasters;
  
    // amor
    private int _currentMagSizebullet;
    private int _totalbullet;
    private int maxMag = 3;

    // bullet plasma
    private const int ID_BULLET_REDPLASMA = 4;
    private const float angleRedPlasma = 15f;
    private static readonly int SHOOT_HASH = Animator.StringToHash("isShoot");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _pointFireTf = _pointFire != null ? _pointFire : transform;
    }

    private void Start()
    {
        _gunData = LoadingData.Instance.GetGunData();
        _gunStatData = LoadingData.Instance.GetGunStatData();

        _paramasters = GunDataReslover.GetParamasters(_gunData, _gunStatData);
       
        _totalbullet = _paramasters.magSize * maxMag;
        _currentMagSizebullet = _paramasters.magSize;

        OnActionCurrentBullet?.Invoke(_currentMagSizebullet);
        OnActionTotalBullet?.Invoke(_totalbullet);

        CheckFirstPlay();
    }

    private void Update()
    {
        if (InputManager.isInputLocked) return;

        if (PlayerSkill.fire && _currentMagSizebullet > 0 && FireRate.canShoot)
        {
            AudioSFX.Instance.PlayAudioOneShortChangeVolume(GetAudioClipsShoting(), _upVolume);
            AudioSFX.Instance.PlayAudioOneShortChangeVolume(GetAudioClipsShellCas(), _upVolume + 1f);

            _animator.SetTrigger(SHOOT_HASH);
            StartCoroutine(FireRoutine());
        }
    }

    private void CheckFirstPlay()
    {
        int first = PlayerPrefs.GetInt(DataPlayerPrefs.fistPlay, 0);
        if (first == 0)
            _currentMagSizebullet = 10000;
    }

    private List<AudioClip> GetAudioClipsShoting() => _audioSFXDataShoting == null ? null : _audioSFXDataShoting.clipList;
    private List<AudioClip> GetAudioClipsShellCas() => _audioShellCas == null ? null : _audioShellCas.clipList;


    private IEnumerator FireRoutine()
    {
        FireRate.canShoot = false;
        _currentMagSizebullet -= 1;

        //Event
        OnActionCurrentBullet?.Invoke(_currentMagSizebullet);

        if(_paramasters.currentGun.bulletPrefabs == null)
        {
            Debug.Log("Bullet null");
            yield break;
        }

        SpawnBullet(_paramasters.currentGun?.bulletPrefabs, _pointFireTf.position, 
                    _pointFireTf.rotation, _pointFireTf.right, _paramasters.bulletSpeed);

        if (_paramasters.currentGun != null && _paramasters.currentGun.idGun == ID_BULLET_REDPLASMA)
        {
            SpawnExtraRedPlasma();
        }

        CreateTrash(_prefabCasing, _positionSpawnCasing, _positionGun);

        if (_currentMagSizebullet <= 0)
        {
            StartCoroutine(ReloadCoroutine());
            yield break;
        }
        yield return new WaitForSeconds(Mathf.Max(0.01f, _paramasters.fireRate));
        FireRate.canShoot = true;
        PlayerSkill.fire = false;
    }



    private void SpawnBullet(GameObject prefab, Vector3 pos,  Quaternion rot, Vector3 dir, float speed)
    {
        if(prefab == null) return;
        GameObject bullet = PoolManager.Instance.Spawner(prefab, pos, rot);
        var ctrl = bullet.GetComponent<BulletBase>();
        ctrl?.Init(dir, speed);
    }

    private void SpawnExtraRedPlasma()
    {
        float[] angles = { angleRedPlasma, -angleRedPlasma };
        foreach (var angle in angles)
        {
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * _pointFireTf.right;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * _pointFireTf.rotation;
            SpawnBullet(_paramasters.currentGun.bulletPrefabs, _pointFireTf.position, rot, dir, _paramasters.bulletSpeed);
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        InputManager.isInputLocked = true;
        CreateTrash(_prefabMagazine, _positionSpawnMagazine, _positionGun);

        int amountToLoad = Mathf.Min(_paramasters.magSize, _totalbullet);
        _currentMagSizebullet = amountToLoad;
        _totalbullet = Mathf.Max(0, _totalbullet - amountToLoad);

        //Event
        OnActionTotalBullet?.Invoke(_totalbullet);
        OnActionCurrentBullet?.Invoke(_currentMagSizebullet);

        //Debug.Log("TotalBullet : " + _totalbullet);
        FireRate.canShoot = true;
        
        if(_currentMagSizebullet <= 0f && _totalbullet <= 0f)
        {
            Debug.Log("Out Ammo");
            InputManager.isInputLocked = true;
            Die?.Invoke();
        }
        else
        {
            OnActionReloading?.Invoke(_paramasters.timeReload);
            yield return new WaitForSeconds(Mathf.Max(0f, _paramasters.timeReload));
            InputManager.isInputLocked = false;
            PlayerSkill.fire = false;
        }

    }

    private void CreateTrash(GameObject trashPrefab, Transform pointSpawn, Transform pointGun)
    {
        Vector2 dir = (pointSpawn.position - pointGun.position).normalized;
        GameObject trash = PoolManager.Instance.Spawner(trashPrefab, pointSpawn.position, Quaternion.identity);
        var trashInit = trash.GetComponent<TrashGun>();
        if (trashInit != null) trashInit.Init(dir);
    }

    private void OnEnable()
    {
        BoxAmor.OnBoxBroken += HandleTakeAmor;
        PlayerSkill.OnSkillShoot  = FireRoutineSkill;
        ShopGunScreenUI.UpdateData += UpdateData;
    }

    private void OnDisable()
    {
        BoxAmor.OnBoxBroken -= HandleTakeAmor;
        PlayerSkill.OnSkillShoot = null;
        ShopGunScreenUI.UpdateData -= UpdateData;
    }    

    private void UpdateData()
    {
        _gunStatData = LoadingData.Instance.GetGunStatData();
        _paramasters = GunDataReslover.GetParamasters(_gunData, _gunStatData);
        Debug.Log("Update Data");
    }    

    private void HandleTakeAmor(int amor)
    {
        _totalbullet += amor;

        // Event
        OnActionTotalBullet?.Invoke(_totalbullet);
        //Debug.Log("Amor last : " + _totalbullet);
    }

    private void FireRoutineSkill()
    {
        SpawnBullet(_paramasters.currentGun?.bulletPrefabs, _pointFireTf.position,
                    _pointFireTf.rotation, _pointFireTf.right, _paramasters.bulletSpeed);

        AudioSFX.Instance.PlayAudioOneShortChangeVolume(GetAudioClipsShoting(), _upVolume + 0.5f);
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(GetAudioClipsShellCas(), _upVolume + 1f);

        if (_paramasters.currentGun != null && _paramasters.currentGun.idGun == ID_BULLET_REDPLASMA)
        {
            SpawnExtraRedPlasma();
        }

        CreateTrash(_prefabCasing, _positionSpawnCasing, _positionGun);
    }


}
