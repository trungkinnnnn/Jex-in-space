using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/DataList")]
public class AchievementDataList : ScriptableObject
{
   public List<Achievement> achievements;
}

[System.Serializable]
public class Achievement
{
    public AchievementType type;
    public int idAchi;
    public string gpgId;
    public string description;
    public int min;
    public int max;
    public int coin;
    public bool completed;
    public bool claimed;
    public bool reset;
    public bool secret;
}

public enum AchievementType
{
    DestroyAsteroid,
    WaveSurvive,
    TimeLife,
}