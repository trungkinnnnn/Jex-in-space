
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner
{
    private readonly WaveData waveData;
    private readonly WaveConfig waveConfig;
    private readonly Transform playerPosition;
    private readonly RectangSpawner rectangSpawner = new RectangSpawner();
    private readonly List<GameObject> listTrackerAst = new List<GameObject>();
    private readonly int speedGame;
    public WaveSpawner(WaveData waveData, WaveConfig waveConfig, Transform playerPosition, int speedGame)
    {
        this.waveData = waveData;
        this.waveConfig = waveConfig;
        this.playerPosition = playerPosition;   
        this.speedGame = speedGame;
    }

    public bool IsWaveCleared() => listTrackerAst.Count == 0;

    public List<SpawnType> GenerateSpawnList()
    {
        var list = new List<SpawnType>();
        SpawnUtility.AddMultiple(list, SpawnType.BigAst, waveConfig.bigAts.totalCount, speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.MediumAst, waveConfig.mediumAts.totalCount, speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.SmallAst, waveConfig.smallAts.totalCount, speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.GoldAst, waveConfig.goldAts.totalCount, speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.ItemHealth, waveConfig.itemHealth.totalCount, speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.AmorBox, waveConfig.amorBox.totalCount, speedGame);

        SpawnUtility.Shuffle(list);
        return list;
    }

    public IEnumerator SpawnWave(List<SpawnType> shuffer)
    {
        int index = 0;

        while (index < shuffer.Count)
        {
            int batchSize = Mathf.Min(waveConfig.totalAst, shuffer.Count - index);

            for(int i = 0; i < batchSize; i++)
            {
                SpawnType type = shuffer[i + index];
                GameObject prefabs = PrefabSelector.GetRandomPrefabs(type, waveData);
                Vector3 spawnPoint = rectangSpawner.GetSpawnPoint(i);

                GameObject ast = Object.Instantiate(prefabs, spawnPoint, Quaternion.identity);
                ast.GetComponent<AstMovement>()?.SetTranformPlayer(playerPosition);

                if(SpawnUtility.IsTrackAst(type))
                {
                    listTrackerAst.Add(ast);
                    AstTracker astTracker = ast.GetComponent<AstTracker>() ?? ast.AddComponent<AstTracker>();
                    astTracker.Init(() => listTrackerAst.Remove(ast));  
                }

                Debug.Log("listTracker : " + listTrackerAst.Count);

                yield return new WaitForSeconds(SpawnUtility.GetDelayForType(type, waveConfig));
            }

            index += batchSize;
            yield return new WaitForSeconds(waveConfig.spawnIntervalTime);

        }

    }

}
