
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static Action<int> GetWave;

    [Header("Wave Data")]
    [SerializeField] WaveData _waveData;
    [SerializeField] WaveConfig _waveConfig;

    [Header("References")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] TextMeshProUGUI _textMeshProAmorBox;
    [SerializeField] Transform _asteroiHolder;

    [Header("GamePlay")]
    [SerializeField] int _gameSpeed;
    [SerializeField] float _delayStart = 5f;

    private WaveSpawner _waveSpawner;
    private Camera _camera; 
    private int _currentWave = 0;
    private int _alphaStart = 0;
    private bool _isRunning = false;    

    private void Start()
    {
        _camera = Camera.main;
        SetTextAlphaZero();
        StartCoroutine(StartAfterDelay(_delayStart));
    }

    private void SetTextAlphaZero()
    {
        Color color = _textMeshProAmorBox.color;
        color.a = _alphaStart;
        _textMeshProAmorBox.color = color;
    }    

    private IEnumerator StartAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _waveSpawner = new WaveSpawner(_waveData, _waveConfig, _playerTransform, _asteroiHolder, _textMeshProAmorBox, _camera, _gameSpeed);
        _isRunning = true;
        StartCoroutine(WaveLoop());
    }    


    private IEnumerator WaveLoop()
    {
        while (_isRunning)
        {
            _currentWave++;
            Debug.Log("Wave : " + _currentWave);
            GetWave?.Invoke(_currentWave);

            List<(SpawnType, int score)> spawnList = _waveSpawner.GenerateSpawnList();
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
        _waveConfig.explosionAst.totalCount += _waveConfig.explosionAst.countUpNextWave;
        _waveConfig.itemHealth.totalCount += _waveConfig.itemHealth.countUpNextWave;
        _waveConfig.amorBox.totalCount += _waveConfig.amorBox.countUpNextWave;

        if(wave % 3 == 0)
        {
            _waveConfig.spawnIntervalTime = Mathf.Max(_waveConfig.spawnIntervalTime - _waveConfig.spawnDownEvery3Wave
                                                        , _waveConfig.minSpawnInterval);
        }    

    }

}