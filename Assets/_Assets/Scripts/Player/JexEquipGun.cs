using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JexEquipGun : MonoBehaviour
{
    // data
    [SerializeField] GunData gunData;

    [SerializeField] Transform gunPostion;

    private void Start()
    {
        foreach(var stat in gunData.gunStats)
        {
            if(stat.unlock == true)
            {
                Instantiate(stat.gunPrefabs, gunPostion);
                break;
            }    
        }    
    }

}
