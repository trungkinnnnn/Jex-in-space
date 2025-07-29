using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
                GameObject gun =  Instantiate(stat.gunPrefabs, gunPostion);
                break;
            }    
        }    
    }

  

}
