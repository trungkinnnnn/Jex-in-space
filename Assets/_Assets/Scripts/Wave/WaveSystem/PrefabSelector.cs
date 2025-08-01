
using System.Collections.Generic;
using UnityEngine;

public static class PrefabSelector
{
    public static GameObject GetRandomPrefabs(SpawnType spawnType, WaveData data)
    {
        return spawnType switch
        {
            SpawnType.BigAst => GetRandomFrom(data.bigAts),
            SpawnType.MediumAst => GetRandomFrom(data.mediumAts),
            SpawnType.SmallAst => GetRandomFrom(data.smallAts),
            SpawnType.GoldAst => GetRandomFrom(data.goldAts),
            SpawnType.ItemHealth => GetRandomFrom(data.itemHealth),
            SpawnType.AmorBox => GetRandomFrom(data.amorBox),   
            _ => null
        };
    }

    private static GameObject GetRandomFrom(List<GameObject> list)
    {
        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }
}
