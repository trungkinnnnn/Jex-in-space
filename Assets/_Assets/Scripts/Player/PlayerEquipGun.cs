using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerEquipGun : MonoBehaviour
{
    // data
    [SerializeField] Transform _gunPostion;

    private LoadData _loadData;
    private GunData _gunData;

    private void Start()
    {
        _loadData = LoadData.Instance;
        _gunData = _loadData.GetGunData();

        var unlockedGun = _gunData.gunStats.FirstOrDefault(stat => stat.equip);
        if (unlockedGun != null)
        {
            Instantiate(unlockedGun.gunPrefabs, _gunPostion);
        }
    }


}
