using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName =("Data/LogoScipTable"))]
public class LogoScripTable : ScriptableObject
{
    public float addForce_max;
    public float addForce_min;
    public float addForceTorque_max;
    public float addForceTorque_min;
    public float timeReset;
    public float timeStart;
}
