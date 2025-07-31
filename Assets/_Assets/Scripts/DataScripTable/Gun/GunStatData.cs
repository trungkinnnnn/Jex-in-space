
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
    public List<DataLevel> magSize;
    public List<DataLevel> bulletSpeed;
    public List<DataLevel> timeReload;
    public List<DataLevel> fireRate;
}

[System.Serializable]
public class DataLevel
{
    public string name;
    public int level;
    public float value;
    public float price;
    public bool unlock;
}