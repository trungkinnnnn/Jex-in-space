using System.Collections;
using TMPro;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GunData _gunData;
    [SerializeField] GunStatData _gunStatData;
    [SerializeField] Transform _pointFire;

    [Header("Trash")]        
    [SerializeField] Transform _positionGun;
    [SerializeField] Transform _positionSpawnCasing;
    [SerializeField] Transform _positionSpawnMagazine;
    [SerializeField] GameObject _prefabCasing;
    [SerializeField] GameObject _prefabMagazine;
    

    // runtime
    private Animator _animator;
    private Transform _pointFireTf;
    private GunParamasters _paramasters;
  
    // amor
    private float _currentMagSizebullet;
    private float _totalbullet;
    private float maxMag = 3f;

    // bullet plasma
    private const string ID_BULLET_REDPLASMA = "Gun05";
    private const float angleRedPlasma = 15f;
    private static readonly int SHOOT_HASH = Animator.StringToHash("isShoot");

    private void Awake()
    {
        _animator = GetComponent<Animator>();   
        _pointFireTf = _pointFire != null ? _pointFire : transform;
    }

    private void Start()
    {
        _paramasters = GunDataReslover.GetParamasters(_gunData, _gunStatData);

        _totalbullet = _paramasters.magSize * maxMag;
        _currentMagSizebullet = _paramasters.magSize;
    }

    private void Update()
    {
        if (InputManager.isInputLocked) return;

        if (Input.GetMouseButtonDown(0) && _currentMagSizebullet > 0 && FireRate.canShoot)
        {
            _animator.SetTrigger(SHOOT_HASH);
            StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        FireRate.canShoot = false;

        _currentMagSizebullet -= 1;

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
    }

    private void SpawnBullet(GameObject prefab, Vector3 pos,  Quaternion rot, Vector3 dir, float speed)
    {
        if(prefab == null) return;
        GameObject bullet = Instantiate(prefab, pos, rot);
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
        yield return new WaitForSeconds(Mathf.Max(0f, _paramasters.timeReload));

        float amountToLoad = Mathf.Min(_paramasters.magSize, _totalbullet);
        _currentMagSizebullet = amountToLoad;
        _totalbullet = Mathf.Max(0f, _totalbullet - amountToLoad);

        Debug.Log("TotalBullet : " + _totalbullet);

        FireRate.canShoot = true;
        InputManager.isInputLocked = false;

        if(_currentMagSizebullet <= 0f && _totalbullet <= 0f)
        {
            Debug.Log("Out Ammo");
        }

    }

    private void CreateTrash(GameObject trashPrefab, Transform pointSpawn, Transform pointGun)
    {
        Vector2 dir = (pointSpawn.position - pointGun.position).normalized;
        GameObject trash = Instantiate(trashPrefab, pointSpawn.position, Quaternion.identity);
        var trashInit = trash.GetComponent<TrashGun>();
        if (trashInit != null) trashInit.Init(dir);
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
        _totalbullet += amor;
        Debug.Log("Amor last : " + _totalbullet);
    }    

}
