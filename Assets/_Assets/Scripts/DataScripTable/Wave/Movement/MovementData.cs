using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/Movement"))]
public class MovementData : ScriptableObject
{
    public float addForceMax;
    public float addForceMin;
    public float addForceTorqueMax;
    public float addForceTorqueMin;

    public float addForceOutScreen;

    public float minSpeed;
    public float timeBack;
}
