using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] AchievementDataList _achievementDataList;

    [Header("Transform")]
    [SerializeField] GameObject _parentContent;
    private RectTransform _reTransform;

    [Header("ContentPrefabs")]
    [SerializeField] GameObject _content;

    private int _countDestroyAst;

    public float plusHeightContent;
    public static AchievementManager Instace;
    private void Awake()
    {
        if(Instace == null) Instace = this; ;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void RegisterEvents()
    {
        Ast.AddScoreOnDie += HandleCountDestroyAsteroid;
    }    

    private void UnRegisterEvents()
    {
        Ast.AddScoreOnDie -= HandleCountDestroyAsteroid;
    }

    private void Start()
    {
        _reTransform = _parentContent.GetComponent<RectTransform>();
        _countDestroyAst = PlayerPrefs.GetInt(DataPlayerPrefs.para_COUNT_DESTROYASTEROID, 0);

        DeleteChildTransform();
        SetUp();
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
        if (_achievementDataList == null || _achievementDataList.achievements.Count <= 0) return;
        foreach(Achievement achi in _achievementDataList.achievements)
        {
            CreateAchiChild(achi);
            PlusHeight();
        }    
    }    

    private void CreateAchiChild(Achievement achi)
    {
        var content = Instantiate(_content, _parentContent.transform);
        AchievementContent achiContent = content.GetComponent<AchievementContent>();
        achiContent.Init(achi);
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
    }
    

}
