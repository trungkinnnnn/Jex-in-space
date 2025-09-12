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
    public bool secret;
    public bool success;
    public bool reset;
}

public enum AchievementType
{
    DestroyAsteroid,
    WaveSurvive,
    TimeLife,
}