
using UnityEngine;

[CreateAssetMenu(menuName = ("Wave/WaveConfigData"))]
public class WaveConfig : ScriptableObject
{
    public SpawnUnitConfig bigAts;
    public SpawnUnitConfig mediumAts;
    public SpawnUnitConfig smallAts;
    public SpawnUnitConfig goldAts;
    public SpawnUnitConfig itemHealth;
    public SpawnUnitConfig amorBox;

    public float spawnIntervalTime = 10f;
    public float spawnDownEvery3Wave = 0.5f;
    public float minSpawnInterval = 5f;
}


[System.Serializable]
public class SpawnUnitConfig
{
    public int totalCount;
    public int perBatch;

    public int countUpNextWave;
    public int countUpPerNextWave;

    public int maxPerBatch = 10;

    public float delayBetweenBatch = 1f;
}