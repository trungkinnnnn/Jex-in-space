using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/GunData"))]
public class GunData : ScriptableObject
{
    public List<GunStat> gunStats;
}

[System.Serializable]
public class GunStat
{
    public int idGun;
    public string nameGun;
    public int priceCoin;
    public int priceMoney;
    public bool unlock;
    public bool equip;

    public GameObject gunPrefabs;
    public GameObject bulletPrefabs;
}