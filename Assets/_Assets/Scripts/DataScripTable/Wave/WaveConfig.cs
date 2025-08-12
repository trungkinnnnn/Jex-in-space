
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

    public float spawnIntervalTime = 7f;
    public float spawnDownEvery3Wave = 0.5f;
    public float minSpawnInterval = 5f;

    public int totalAst = 30;
    public int upTotalNextWave = 5;

    public float timeNextWave = 10f;
}


[System.Serializable]
public class SpawnUnitConfig
{
    public int totalCount;
    public int countUpNextWave;

    public float delayBetweenBatch = 1f;

    public int score = 1;
}