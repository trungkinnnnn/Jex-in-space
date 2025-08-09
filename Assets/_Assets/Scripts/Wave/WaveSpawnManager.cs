
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawnManager : MonoBehaviour
{
    [SerializeField] WaveData _waveData;
    [SerializeField] WaveConfig _waveConfig;
    [SerializeField] Transform _playerPosition;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] int _speedGame;
    [SerializeField] float _timeStart = 5f;

    private WaveSpawner _waveSpawner;
    private int _currentWave = 0;
    private int _alphaStart = 0;

    private void Start()
    {
        SetAlphaZero();
        StartCoroutine(TimeStart(_timeStart));
    }

    private void SetAlphaZero()
    {
        Color color = _textMeshProUGUI.color;
        color.a = _alphaStart;
        _textMeshProUGUI.color = color;
    }    

    private IEnumerator TimeStart(float time)
    {
        yield return new WaitForSeconds(time);
        _waveSpawner = new WaveSpawner(_waveData, _waveConfig, _playerPosition, _textMeshProUGUI, _speedGame);
        StartCoroutine(WaveLoop());
    }    


    private IEnumerator WaveLoop()
    {
        while (true)
        {
            _currentWave++;
            Debug.Log("Wave : " + _currentWave);    

            List<SpawnType> spawnList = _waveSpawner.GenerateSpawnList();
            yield return StartCoroutine(_waveSpawner.SpawnWave(spawnList));

            yield return new WaitUntil(() => _waveSpawner.IsWaveCleared());

            Update_waveConfig(_currentWave);

            yield return new WaitForSeconds(_waveConfig.timeNextWave);

        }
    }


    private void Update_waveConfig(int wave)
    {
        _waveConfig.bigAts.totalCount += _waveConfig.bigAts.countUpNextWave;
        _waveConfig.mediumAts.totalCount += _waveConfig.mediumAts.countUpNextWave;
        _waveConfig.smallAts.totalCount += _waveConfig.smallAts.countUpNextWave;
        _waveConfig.goldAts.totalCount += _waveConfig.goldAts.countUpNextWave;
        _waveConfig.itemHealth.totalCount += _waveConfig.itemHealth.countUpNextWave;
        _waveConfig.amorBox.totalCount += _waveConfig.amorBox.countUpNextWave;

        if(wave % 3 == 0)
        {
            _waveConfig.spawnIntervalTime = Mathf.Max(_waveConfig.spawnIntervalTime - _waveConfig.spawnDownEvery3Wave
                                                        , _waveConfig.minSpawnInterval);
        }    

    }

}