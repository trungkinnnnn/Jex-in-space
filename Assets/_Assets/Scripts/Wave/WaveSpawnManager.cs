using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private int currentWave = 0;
    List<GameObject> listTrackerAst;

    private void Start()
    {
        rectangSpawner = new RectangSpawner();
        listTrackerAst = new List<GameObject>();
        StartCoroutine(WaveLoop());
    }
    
    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;
            Debug.Log("Wave : " +  currentWave);
            List<SpawnType> spawnList = GenerateSpawnList(waveConfig);

            yield return StartCoroutine(SpawnWave(spawnList, waveData, waveConfig));

            yield return new WaitUntil(() => listTrackerAst.Count == 0);    

            UpdateWaveConfig(currentWave, waveConfig);

            yield return new WaitForSeconds(waveConfig.spawnIntervalTime);
        }
    }

    private void UpdateWaveConfig(int wave, WaveConfig waveConfig)
    {
     
        waveConfig.bigAts.totalCount += waveConfig.bigAts.countUpNextWave;
        waveConfig.mediumAts.totalCount += waveConfig.mediumAts.countUpNextWave;
        waveConfig.smallAts.totalCount += waveConfig.smallAts.countUpNextWave;
        waveConfig.goldAts.totalCount += waveConfig.goldAts.countUpNextWave;
        waveConfig.itemHealth.totalCount += waveConfig.itemHealth.countUpNextWave;
        waveConfig.amorBox.totalCount += waveConfig.amorBox.countUpNextWave;

        waveConfig.totalAst += waveConfig.upTotalNextWave;

        if (wave % 3 == 0)
        {
            waveConfig.spawnIntervalTime = Mathf.Max(waveConfig.spawnIntervalTime - waveConfig.spawnDownEvery3Wave, waveConfig.minSpawnInterval);
        }
    }

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
        for(int i = 0; i < count/2; i+=1)
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

        while(index < shuffle.Count)
        {
            int batchSize = Mathf.Min(waveConfig.totalAst, shuffle.Count - index);

            for(int i = 0;i < batchSize;i+=1)
            {
                SpawnType spawnType = shuffle[index + i];
                GameObject prefab = GetRandomPrefabs(spawnType, waveData);

                Vector3 spawnPoint = rectangSpawner.GetSpawnPoint(i);
                GameObject ast =  Instantiate(prefab, spawnPoint, Quaternion.identity);
                var setPostion = ast.GetComponent<SpaceMovement>();
                setPostion.SetTranformPlayer(player);
                if(CheckTypeAst(spawnType))
                {
                    listTrackerAst.Add(ast);
                    AstTracker astTracker = ast.GetComponent<AstTracker>(); 

                    if(astTracker == null) astTracker.AddComponent<AstTracker>();

                    astTracker.Init(() => listTrackerAst.Remove(ast));
                }    
                
                Debug.Log("ListTracker : " + listTrackerAst.Count); 

                yield return new WaitForSeconds(GetDelayForType(spawnType, waveConfig));
            }
            index += batchSize;

            yield return new WaitForSeconds(waveConfig.timeNextWave);
        }    
    }

    //Checkking loại Ast
    private bool CheckTypeAst(SpawnType spawnType)
    {
        if(spawnType == SpawnType.BigAts ||
            spawnType == SpawnType.MediumAts ||
            spawnType == SpawnType.SmallAts)
        {
            return true;
        }    
        return false;
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
