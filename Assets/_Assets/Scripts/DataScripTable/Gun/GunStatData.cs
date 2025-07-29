using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/GunStatData"))]
public class GunStatData : ScriptableObject
{
    public List<StatLevel> statLevels;
}

[System.Serializable]
public class StatLevel
{
    public string idGun;
    public string nameStat;
    public DataLevel stats;
}

[System.Serializable]
public class DataLevel
{
    public int level;
    public float value;
    public float price;
    public bool unlock;
}