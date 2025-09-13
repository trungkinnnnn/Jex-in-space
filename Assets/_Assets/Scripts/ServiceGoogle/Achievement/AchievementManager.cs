
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


public class AchievementManager : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] GameObject _parentContent;
    private RectTransform _reTransform;

    [Header("ContentPrefabs")]
    [SerializeField] GameObject _content;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    [Header("Button")]
    [SerializeField] Button _showAchievementBtn;

    [Header("=======================")]
    private List<Achievement> _dataAchiClone;

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
        _dataAchiClone = LoadingData.Instance.GetAchievementDataList().achievements;
        _showAchievementBtn.onClick.AddListener(() => ShowAchievement());

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
        if (_dataAchiClone == null || _dataAchiClone.Count <= 0) return;
        foreach(Achievement achi in _dataAchiClone)
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

        for(int i = 0; i < _dataAchiClone.Count; i++)
        {
            if (_dataAchiClone[i].type != AchievementType.DestroyAsteroid 
                || _dataAchiClone[i].completed) continue;

            _dataAchiClone[i].min = _countDestroyAst;
            if(_countDestroyAst == _dataAchiClone[i].max)
            {
                _dataAchiClone[i].completed = true;
                SaveAchievementData();
                UnlockAchievement(_dataAchiClone[i].gpgId, _dataAchiClone[i].description);
            }
        }
    }

    public int GetCountDestroy() => _countDestroyAst;

    private void HandleCountWave(int wave)
    {
       for(int i = 0; i < _dataAchiClone.Count; i++)
       {
            if (_dataAchiClone[i].type != AchievementType.WaveSurvive || _dataAchiClone[i].completed) continue;

            _dataAchiClone[i].min = wave;
            if(wave == _dataAchiClone[i].max)
            {
                _dataAchiClone[i].completed = true;
                SaveAchievementData();
                UnlockAchievement(_dataAchiClone[i].gpgId, _dataAchiClone[i].description);
            }
       }    

    }

    private void HandleTimeLife(int hp)
    {
        for(int i = 0; i < _dataAchiClone.Count; i++)
        {
            if (_dataAchiClone[i].type != AchievementType.TimeLife || _dataAchiClone[i].completed) continue;
            
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
        for(int i = 0; i < _dataAchiClone.Count;i++)
        {
            if (_dataAchiClone[i].type != AchievementType.TimeLife || _dataAchiClone[i].completed) continue;

            _dataAchiClone[i].min = (int)_timeLife;
            if((int)_timeLife == _dataAchiClone[i].max)
            {
                _dataAchiClone[i].completed=true;
                _dataAchiClone[i].secret = false;
                SaveAchievementData();
                UnlockAchievement(_dataAchiClone[i].gpgId, _dataAchiClone[i].description);
            }
            checkAchi = true;
        }

        if(!checkAchi) _startCountTime = false ;

    }    

    public void SaveAchievementData()
    {
        string saveAchi = $"{_dataAchiClone[0].idAchi}," +
                            $"{_dataAchiClone[0].completed}," +
                            $"{_dataAchiClone[0].claimed}," +
                            $"{_dataAchiClone[0].secret}";
        for (int i = 1; i < _dataAchiClone.Count; i++)
        {
            saveAchi += $",{_dataAchiClone[i].idAchi}," +
                            $"{_dataAchiClone[i].completed}," +
                            $"{_dataAchiClone[i].claimed}," +
                            $"{_dataAchiClone[i].secret}";
        }

        PlayerPrefs.SetString(DataPlayerPrefs.para_ACHIEVEMENTLIST, saveAchi);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_COUNT_DESTROYASTEROID, _countDestroyAst);
        PlayerPrefs.Save();
    }

    private void UnlockAchievement(string idGPG, string description)
    {
        if (!Social.localUser.authenticated) return;
        Social.ReportProgress(idGPG, 100.0f, success => { Debug.Log("Success " + description);});
    }    

    private void ShowAchievement()
    {
        if (!Social.localUser.authenticated) return;
        Social.ShowAchievementsUI();
    }    

}
