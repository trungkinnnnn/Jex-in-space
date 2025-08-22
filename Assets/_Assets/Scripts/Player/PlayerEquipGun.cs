
using System.Linq;
using UnityEngine;


public class PlayerEquipGun : MonoBehaviour
{
    // data
    [SerializeField] Transform _gunPostion;

    private GunData _gunData;

    private void OnEnable()
    {
        LoadingData.OnLoadData += GetData;
    }

    private void OnDisable()
    {
        LoadingData.OnLoadData -= GetData;
    }

    private void GetData()
    {
        _gunData = LoadingData.Instance.GetGunData();
       
    }

    private void Start()
    {
        if (_gunData == null)
            _gunData = LoadingData.Instance.GetGunData();
        SetUpGun();
    }

    private void SetUpGun()
    {
        if (_gunData == null)
            _gunData = LoadingData.Instance.GetGunData();
        var unlockedGun = _gunData.gunStats.FirstOrDefault(stat => stat.equip);
        Debug.Log("id" + unlockedGun.idGun);
        if (unlockedGun != null)
        {
            Instantiate(unlockedGun.gunPrefabs, _gunPostion);
        }
    }    

}
