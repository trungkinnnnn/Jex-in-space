using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnUtility
{
   public static void AddMultiple(List<(SpawnType, int score)> list, SpawnType spawnType, int count,int score, int speedGame)
    {
        int loops = Mathf.Max(0, count / Mathf.Max(1, speedGame));
        for(int i = 0; i < loops; i++)
        {
            list.Add((spawnType, score));
        }    
    }

    public static void Shuffle<T>(List<T> list)
    {
        for(int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }    
    }    

    public static bool IsTrackAst(SpawnType spawnType)
    {
        return spawnType == SpawnType.BigAst || spawnType == SpawnType.MediumAst || spawnType == SpawnType.SmallAst || spawnType == SpawnType.ExplosionAst;
    }

    public static float GetDelayForType(SpawnType spawnType, WaveConfig waveConfig)
    {
        return spawnType switch
        {
            SpawnType.BigAst => waveConfig.bigAts.delayBetweenBatch,
            SpawnType.MediumAst => waveConfig.mediumAts.delayBetweenBatch,
            SpawnType.SmallAst => waveConfig.smallAts.delayBetweenBatch,
            SpawnType.GoldAst => waveConfig.goldAts.delayBetweenBatch,
            SpawnType.ExplosionAst => waveConfig.explosionAst.delayBetweenBatch,
            SpawnType.ItemHealth => waveConfig.itemHealth.delayBetweenBatch,
            SpawnType.AmorBox => waveConfig.amorBox.delayBetweenBatch,
            _ => 0.5f
        };
    }

}
