using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

public class AchievementManager : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] GameObject _parentContent;
    private RectTransform _reTransform;

    [Header("ContentPrefabs")]
    [SerializeField] GameObject _content;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    private List<Achievement> _achievementDataListClone;

    private int _countDestroyAst;

    private bool _startCountTime = false;   
    private float _timeStartOneHP;
    private float _timeLife;

    private float _lastTimeCheck = 0f;
    public float timeCheck = 0.8f;


    public float plusHeightContent;
    public static AchievementManager Instace;
    private void Awake()
    {
        if(Instace == null) Instace = this; ;
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents()
    {
        Ast.AddScoreOnDie += HandleCountDestroyAsteroid;
        WaveManager.GetWave += HandleCountWave;
        PlayerHealth.OnActionHp += HandleTimeLife;
    }

    private void UnRegisterEvents()
    {
        Ast.AddScoreOnDie -= HandleCountDestroyAsteroid;
        WaveManager.GetWave -= HandleCountWave;
        PlayerHealth.OnActionHp -= HandleTimeLife;
    }

    private void Start()
    {
        _reTransform = _parentContent.GetComponent<RectTransform>();
        _countDestroyAst = PlayerPrefs.GetInt(DataPlayerPrefs.para_COUNT_DESTROYASTEROID, 0);
        _achievementDataListClone = LoadingData.Instance.GetAchievementDataList().achievements;

        DeleteChildTransform();
        SetUp();
    }

    private void Update()
    {
        if(_startCountTime && Time.time >= _lastTimeCheck)
        {
            CountTimeLife();
            _lastTimeCheck = Time.time + timeCheck;
        }
    }

    private void DeleteChildTransform()
    {
        if(_parentContent != null)
        {
            for(int i = _parentContent.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_parentContent.transform.GetChild(i).gameObject);
            }    
        }    
    } 
    
    private void SetUp()
    {
        if (_achievementDataListClone == null || _achievementDataListClone.Count <= 0) return;
        foreach(Achievement achi in _achievementDataListClone)
        {
            CreateAchiChild(achi);
            PlusHeight();
        }    
    }    

    private void CreateAchiChild(Achievement achi)
    {
        var content = Instantiate(_content, _parentContent.transform);
        AchievementContent achiContent = content.GetComponent<AchievementContent>();
        achiContent.Init(achi, _countDestroyAst, _textTotalCoin);
    }

    private void PlusHeight()
    {
        Vector2 size = _reTransform.sizeDelta;
        size.y += plusHeightContent;
        _reTransform.sizeDelta = size;
    }    


    private void HandleCountDestroyAsteroid(int count, AsteroidType type)
    {
       
        if(type != AsteroidType.AstNon)
        {
            _countDestroyAst += 1;
        }

        for(int i = 0; i < _achievementDataListClone.Count; i++)
        {
            if (_achievementDataListClone[i].type != AchievementType.DestroyAsteroid 
                || _achievementDataListClone[i].completed) continue;

            _achievementDataListClone[i].min = _countDestroyAst;
            if(_countDestroyAst == _achievementDataListClone[i].max)
            {
                _achievementDataListClone[i].completed = true;
                SaveAchievementData();
            }
        }
    }

    private void HandleCountWave(int wave)
    {
       for(int i = 0; i < _achievementDataListClone.Count; i++)
       {
            if (_achievementDataListClone[i].type != AchievementType.WaveSurvive || _achievementDataListClone[i].completed) continue;

            _achievementDataListClone[i].min = wave;
            if(wave == _achievementDataListClone[i].max)
            {
                _achievementDataListClone[i].completed = true;
                SaveAchievementData();
            }
       }    

    }

    private void HandleTimeLife(int hp)
    {
        for(int i = 0; i < _achievementDataListClone.Count; i++)
        {
            if (_achievementDataListClone[i].type != AchievementType.TimeLife || _achievementDataListClone[i].completed) continue;
            
            if(hp == 1)
            {
                _startCountTime = true;
                _timeStartOneHP = Time.time;
            }else
            {
                _startCountTime = false;
            }
        }
    }

    private void CountTimeLife()
    {
        _timeLife = Time.time - _timeStartOneHP;
        bool checkAchi = false;
        Debug.Log("TimeLife with onehp : " +  _timeLife);   
        for(int i = 0; i < _achievementDataListClone.Count;i++)
        {
            if (_achievementDataListClone[i].type != AchievementType.TimeLife || _achievementDataListClone[i].completed) continue;

            _achievementDataListClone[i].min = (int)_timeLife;
            if((int)_timeLife == _achievementDataListClone[i].max)
            {
                _achievementDataListClone[i].completed=true;
                _achievementDataListClone[i].secret = false;
                SaveAchievementData();
            }
            checkAchi = true;
        }

        if(!checkAchi) _startCountTime = false ;

    }    
    
    public void SaveAchievementData()
    {
        string saveAchi = $"{_achievementDataListClone[0].idAchi}," +
                            $"{_achievementDataListClone[0].completed}," +
                            $"{_achievementDataListClone[0].claimed}," +
                            $"{_achievementDataListClone[0].secret}";
        for (int i = 1; i < _achievementDataListClone.Count; i++)
        {
            saveAchi += $",{_achievementDataListClone[i].idAchi}," +
                            $"{_achievementDataListClone[i].completed}," +
                            $"{_achievementDataListClone[i].claimed}," +
                            $"{_achievementDataListClone[i].secret}";
        }

        PlayerPrefs.SetString(DataPlayerPrefs.para_ACHIEVEMENTLIST, saveAchi);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_COUNT_DESTROYASTEROID, _countDestroyAst);
        PlayerPrefs.Save();
    }

    public int GetCountDestroy() => _countDestroyAst;

}
