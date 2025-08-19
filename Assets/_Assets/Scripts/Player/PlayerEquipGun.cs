using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerEquipGun : MonoBehaviour
{
    // data
    [SerializeField] Transform _gunPostion;

    private GunData _gunData;

    private void OnEnable()
    {
        LoadData.OnLoadData += GetData;
    }

    private void OnDisable()
    {
        LoadData.OnLoadData -= GetData;
    }

    private void GetData()
    {
        _gunData = LoadData.Instance.GetGunData();
        SetUpGun();
    }    

    private void SetUpGun()
    {
        var unlockedGun = _gunData.gunStats.FirstOrDefault(stat => stat.equip);
        Debug.Log("id" + unlockedGun.idGun);
        if (unlockedGun != null)
        {
            Instantiate(unlockedGun.gunPrefabs, _gunPostion);
        }
    }    

}
