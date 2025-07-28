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
    public string idGun;
    public string nameGun;
    public float priceCoin;
    public float priceMoney;
    public bool unlock;
    public bool equip;

    public GameObject gunPrefabs;
}