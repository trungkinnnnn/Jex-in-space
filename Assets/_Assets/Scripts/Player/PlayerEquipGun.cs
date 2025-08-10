using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerEquipGun : MonoBehaviour
{
    // data
    [SerializeField] GunData _gunData;
    [SerializeField] Transform _gunPostion;

    private void Start()
    {
        var unlockedGun = _gunData.gunStats.FirstOrDefault(stat => stat.equip);
        if (unlockedGun != null )
        {
            Instantiate(unlockedGun.gunPrefabs, _gunPostion);
        }
    }

  

}
