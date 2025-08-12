
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner
{
    private readonly WaveData _waveData;
    private readonly WaveConfig _waveConfig;
    private readonly Transform _playerTransform;
    private readonly Transform _asteroiHolder;
    private readonly RectangSpawner _rectangSpawner;
    private readonly List<GameObject> _listTrackerAst = new List<GameObject>();
    private readonly TextMeshProUGUI _textMeshProUGUI;
    private readonly int _speedGame;
    public WaveSpawner(WaveData waveData, WaveConfig waveConfig, Transform playerTransform, Transform asteroiHolder, TextMeshProUGUI text, Camera camera, int speedGame)
    {
        _waveData = waveData;
        _waveConfig = waveConfig;
        _playerTransform = playerTransform;   
        _asteroiHolder = asteroiHolder;
        _speedGame = Mathf.Max(1, speedGame);
        _textMeshProUGUI = text;
        _rectangSpawner = new RectangSpawner(camera);
    }

    public bool IsWaveCleared() => _listTrackerAst.Count == 0;

    public List<(SpawnType, int score)> GenerateSpawnList()
    {
        var list = new List<(SpawnType, int Score)>();
        SpawnUtility.AddMultiple(list, SpawnType.BigAst, _waveConfig.bigAts.totalCount,_waveConfig.bigAts.score, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.MediumAst, _waveConfig.mediumAts.totalCount,_waveConfig.mediumAts.score, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.SmallAst, _waveConfig.smallAts.totalCount, _waveConfig.smallAts.score, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.GoldAst, _waveConfig.goldAts.totalCount, _waveConfig.goldAts.score, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.ItemHealth, _waveConfig.itemHealth.totalCount, _waveConfig.itemHealth.score, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.AmorBox, _waveConfig.amorBox.totalCount, _waveConfig.amorBox.score, _speedGame);

        SpawnUtility.Shuffle(list);
        return list;
    }

    public IEnumerator SpawnWave(List<(SpawnType, int score)> shuffer)
    {
        int index = 0;

        while (index < shuffer.Count)
        {
            int batchSize = Mathf.Min(_waveConfig.totalAst, shuffer.Count - index);

            for(int i = 0; i < batchSize; i++)
            {
                SpawnType type = shuffer[i + index].Item1;
                int score = shuffer[i + index].score;

                CreatPrefabAst(type, score, i);

                 //Debug.Log("listTracker : " + listTrackerAst.Count);

                 yield return new WaitForSeconds(SpawnUtility.GetDelayForType(type, _waveConfig));
            }

            index += batchSize;
            yield return new WaitForSeconds(_waveConfig.spawnIntervalTime);

        }

    }

    private void CreatPrefabAst(SpawnType type, int score, int direction)
    {
        GameObject prefabs = PrefabSelector.GetRandomPrefabs(type, _waveData);
        if (prefabs == null)
        {
            Debug.Log("Prefabs Spawn null");
            return;
        }    
        Vector3 spawnPoint = _rectangSpawner.GetSpawnPoint(direction);

        GameObject ast = Object.Instantiate(prefabs, spawnPoint, Quaternion.identity);

        if (ast.TryGetComponent<AstMovement>(out var movement) && _playerTransform != null)
            movement.SetTranformPlayer(_playerTransform);

        if(ast.TryGetComponent<Ast>(out var astScore))
            astScore.InitAddScore(score);

        AddTracker(ast, type);

        if(type == SpawnType.AmorBox && ast.TryGetComponent<BoxAmor>(out var box))
        {
            box.Init(_textMeshProUGUI);
        }

        if (type != SpawnType.ItemHealth && type != SpawnType.AmorBox)
            ast.transform.SetParent(_asteroiHolder, true);
       
    }
    private void AddTracker(GameObject ast, SpawnType type)
    {
        if (SpawnUtility.IsTrackAst(type))
        {
            _listTrackerAst.Add(ast);
            Ast astTracker = ast.GetComponent<Ast>() ?? ast.AddComponent<Ast>();
            astTracker.InitOnDestroy(() => _listTrackerAst.Remove(ast));
        }
    }

  



}
