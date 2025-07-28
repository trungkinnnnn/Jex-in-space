
using UnityEngine;


[CreateAssetMenu(menuName = ("Data/PlayerData"))]
public class JexData : ScriptableObject
{
    public int health;
    public float addForceMax;
    public float addForceMin;
    public float addForceTorqueInput_Max;
    public float addForceTorqueInput_Min;   
    public float addForceTorqueWind;

    public float timeRecovery;
    public float timeImmotal;
}
