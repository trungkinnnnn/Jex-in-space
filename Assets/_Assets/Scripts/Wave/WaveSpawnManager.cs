﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    BigAts = 1,
    MediumAts = 2,
    SmallAts = 3,
    GoldAts = 4,
    ItemHealth = 5,
    AmorBox = 6
}


public class WaveSpawnManager : MonoBehaviour
{
    [SerializeField] WaveData waveData;
    [SerializeField] WaveConfig waveConfig;
    [SerializeField] Transform player;

    private RectangSpawner rectangSpawner;

    private void Start()
    {
        rectangSpawner = new RectangSpawner();
        StartCoroutine(SpawnWave(GenerateSpawnList(waveConfig), waveData, waveConfig));
    }

    //private void StartSpawn(WaveData waveData, WaveConfig waveConfig)
    //{
    //    this.waveData = waveData;
    //    this.waveConfig = waveConfig;
        
    //}


    public List<SpawnType> GenerateSpawnList(WaveConfig waveConfig)
    {
        List<SpawnType> spawnList = new List<SpawnType>();

        AddMultiple(spawnList, SpawnType.BigAts, waveConfig.bigAts.totalCount);
        AddMultiple(spawnList, SpawnType.MediumAts, waveConfig.mediumAts.totalCount);
        AddMultiple(spawnList, SpawnType.SmallAts, waveConfig.smallAts.totalCount);
        AddMultiple(spawnList, SpawnType.GoldAts, waveConfig.goldAts.totalCount);
        AddMultiple(spawnList, SpawnType.ItemHealth, waveConfig.itemHealth.totalCount);
        AddMultiple(spawnList, SpawnType.AmorBox, waveConfig.amorBox.totalCount);

        Shuffle(spawnList);
        return spawnList;
    }

    private void AddMultiple(List<SpawnType> spawnList, SpawnType spawnType, int count)
    {
        for(int i = 0; i < count; i+=1)
        {
            spawnList.Add(spawnType);
        }
    }    

    private void Shuffle<T>(List<T> list)
    {
        for(int i = list.Count - 1; i > 0; i-=1)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private IEnumerator SpawnWave(List<SpawnType> shuffle, WaveData waveData, WaveConfig waveConfig)
    {
        int index = 0;
        int sumPerBatch = GetSumPerBatch(waveConfig);
        while(index < shuffle.Count)
        {
            int batchSize = Mathf.Min(sumPerBatch, shuffle.Count - index);

            for(int i = 0;i < batchSize;i+=1)
            {
                SpawnType spawnType = shuffle[index + i];
                GameObject prefab = GetRandomPrefabs(spawnType, waveData);

                Vector3 spawnPoint = rectangSpawner.GetSpawnPoint(i);
                GameObject ast =  Instantiate(prefab, spawnPoint, Quaternion.identity);
                var setPostion = ast.GetComponent<SpaceMovement>();
                setPostion.SetTranformPlayer(player);

                yield return new WaitForSeconds(GetDelayForType(spawnType, waveConfig));
            }
            index += batchSize;

            yield return new WaitForSeconds(waveConfig.spawnIntervalTime);
        }    
    }

    private int GetSumPerBatch(WaveConfig waveConfig)
    {
        return waveConfig.bigAts.perBatch + waveConfig.mediumAts.perBatch + waveConfig.smallAts.perBatch + waveConfig.goldAts.perBatch
            + waveConfig.itemHealth.perBatch + waveConfig.amorBox.perBatch;
    }    

    // lấy prefabs từ data
    private GameObject GetRandomPrefabs(SpawnType spawnType, WaveData data)
    {
        switch(spawnType)
        {
            case SpawnType.BigAts: return GetRandomFromList(data.bigAts);
            case SpawnType.MediumAts: return GetRandomFromList(data.mediumAts);
            case SpawnType.SmallAts: return GetRandomFromList(data.smallAts);
            case SpawnType.GoldAts: return GetRandomFromList(data.goldAts);
            case SpawnType.ItemHealth: return GetRandomFromList(data.itemHealth);
            case SpawnType.AmorBox: return GetRandomFromList(data.amorBox);
            default: return null;
        }
    }

    // random prefabs cần lấy
    private GameObject GetRandomFromList(List<GameObject> list)
    {
        if(list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }

    // lấy timedelay 
    private float GetDelayForType(SpawnType spawnType, WaveConfig waveConfig)
    {
        switch(spawnType)
        {
            case SpawnType.BigAts: return waveConfig.bigAts.delayBetweenBatch;
            case SpawnType.MediumAts: return waveConfig.mediumAts.delayBetweenBatch;
            case SpawnType.SmallAts: return waveConfig.smallAts.delayBetweenBatch;
            case SpawnType.GoldAts: return waveConfig.goldAts.delayBetweenBatch;
            case SpawnType.ItemHealth: return waveConfig.itemHealth.delayBetweenBatch;
            case SpawnType.AmorBox: return waveConfig.amorBox.delayBetweenBatch;
            default: return 0.5f;
        }    
    }    

}
