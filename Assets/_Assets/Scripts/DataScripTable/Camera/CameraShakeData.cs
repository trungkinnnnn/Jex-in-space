using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera/ShakeData")]
public class CameraShakeData : ScriptableObject
{
    public float range = 1f;
    public float intensity = 1f;
}
