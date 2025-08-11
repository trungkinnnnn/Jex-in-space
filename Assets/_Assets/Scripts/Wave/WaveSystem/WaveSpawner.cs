
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner
{
    private readonly WaveData _waveData;
    private readonly WaveConfig _waveConfig;
    private readonly Transform _playerPosition;
    private readonly Transform _asteroiHolder;
    private readonly RectangSpawner _rectangSpawner = new RectangSpawner();
    private readonly List<GameObject> _listTrackerAst = new List<GameObject>();
    private readonly TextMeshProUGUI _textMeshProUGUI;
    private readonly int _speedGame;
    public WaveSpawner(WaveData waveData, WaveConfig waveConfig, Transform playerPosition,Transform asteroiHolder, TextMeshProUGUI text, int speedGame)
    {
        _waveData = waveData;
        _waveConfig = waveConfig;
        _playerPosition = playerPosition;   
        _asteroiHolder = asteroiHolder;
        _speedGame = speedGame;
        _textMeshProUGUI = text;    
    }

    public bool IsWaveCleared() => _listTrackerAst.Count == 0;

    public List<SpawnType> GenerateSpawnList()
    {
        var list = new List<SpawnType>();
        SpawnUtility.AddMultiple(list, SpawnType.BigAst, _waveConfig.bigAts.totalCount, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.MediumAst, _waveConfig.mediumAts.totalCount, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.SmallAst, _waveConfig.smallAts.totalCount, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.GoldAst, _waveConfig.goldAts.totalCount, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.ItemHealth, _waveConfig.itemHealth.totalCount, _speedGame);
        SpawnUtility.AddMultiple(list, SpawnType.AmorBox, _waveConfig.amorBox.totalCount, _speedGame);

        SpawnUtility.Shuffle(list);
        return list;
    }

    public IEnumerator SpawnWave(List<SpawnType> shuffer)
    {
        int index = 0;

        while (index < shuffer.Count)
        {
            int batchSize = Mathf.Min(_waveConfig.totalAst, shuffer.Count - index);

            for(int i = 0; i < batchSize; i++)
            {
                SpawnType type = shuffer[i + index];

                CreatPrefabAst(type, i);

                 //Debug.Log("listTracker : " + listTrackerAst.Count);

                 yield return new WaitForSeconds(SpawnUtility.GetDelayForType(type, _waveConfig));
            }

            index += batchSize;
            yield return new WaitForSeconds(_waveConfig.spawnIntervalTime);

        }

    }

    private void CreatPrefabAst(SpawnType type, int direction)
    {
        GameObject prefabs = PrefabSelector.GetRandomPrefabs(type, _waveData);
        Vector3 spawnPoint = _rectangSpawner.GetSpawnPoint(direction);

        GameObject ast = Object.Instantiate(prefabs, spawnPoint, Quaternion.identity);
        ast.GetComponent<AstMovement>()?.SetTranformPlayer(_playerPosition);
        AddTracker(ast, type);

        if(type == SpawnType.AmorBox)
        {
            ast.GetComponent<BoxAmor>()?.Init(_textMeshProUGUI);
        }

        if (type == SpawnType.ItemHealth || type == SpawnType.AmorBox) return;
        ast.transform.SetParent(_asteroiHolder, true);
       
    }
    private void AddTracker(GameObject ast, SpawnType type)
    {
        if (SpawnUtility.IsTrackAst(type))
        {
            _listTrackerAst.Add(ast);
            Ast astTracker = ast.GetComponent<Ast>() ?? ast.AddComponent<Ast>();
            astTracker.Init(() => _listTrackerAst.Remove(ast));
        }
    }

  



}
