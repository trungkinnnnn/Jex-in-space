﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnManager : MonoBehaviour
{
    [SerializeField] WaveData waveData;
    [SerializeField] WaveConfig waveConfig;
    [SerializeField] Transform playerPosition;
    [SerializeField] int speedGame;
    [SerializeField] float timeStart = 5f;

    private WaveSpawner waveSpawner;
    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(TimeStart(timeStart));
    }

    private IEnumerator TimeStart(float time)
    {
        yield return new WaitForSeconds(time);
        waveSpawner = new WaveSpawner(waveData, waveConfig, playerPosition, speedGame);
        StartCoroutine(WaveLoop());
    }    


    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;
            Debug.Log("Wave : " +  currentWave);    

            List<SpawnType> spawnList = waveSpawner.GenerateSpawnList();
            yield return StartCoroutine(waveSpawner.SpawnWave(spawnList));

            yield return new WaitUntil(() => waveSpawner.IsWaveCleared());

            UpdateWaveConfig(currentWave);

            yield return new WaitForSeconds(waveConfig.timeNextWave);

        }
    }


    private void UpdateWaveConfig(int wave)
    {
        waveConfig.bigAts.totalCount += waveConfig.bigAts.countUpNextWave;
        waveConfig.mediumAts.totalCount += waveConfig.mediumAts.countUpNextWave;
        waveConfig.smallAts.totalCount += waveConfig.smallAts.countUpNextWave;
        waveConfig.goldAts.totalCount += waveConfig.goldAts.countUpNextWave;
        waveConfig.itemHealth.totalCount += waveConfig.itemHealth.countUpNextWave;
        waveConfig.amorBox.totalCount += waveConfig.amorBox.countUpNextWave;

        if(wave % 3 == 0)
        {
            waveConfig.spawnIntervalTime = Mathf.Max(waveConfig.spawnIntervalTime - waveConfig.spawnDownEvery3Wave, waveConfig.minSpawnInterval);
        }    

    }

}