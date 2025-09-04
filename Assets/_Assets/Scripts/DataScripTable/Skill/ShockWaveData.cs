
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/ShockWaveData")]
public class ShockWaveData : ScriptableObject
{
    public float waveStartMat = -0.1f;
    public float waveEndMat = 0.5f;
    public float radiusColliderStart = 0f;
    public float radiusColliderEnd = 0.08f;
    public float duration = 2f;
    public float forceEnter = 0.1f;
}
