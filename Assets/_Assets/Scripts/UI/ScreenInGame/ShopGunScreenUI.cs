using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopGunScreenUI : MonoBehaviour
{

    public Action<int> actionSelected;
    public Action<StatType> actionUpdateLevel;

    [Header("TextUI Coin")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;
    [SerializeField] TextMeshProUGUI _textCoinBuyGun;
    [SerializeField] List<TextMeshProUGUI> _textCoinUpdateLevels;

    [Header("Gun")]
    [SerializeField] TextMeshProUGUI _textNameGun;
    [SerializeField] Image _imageGun;
    [SerializeField] List<Sprite> _spriteGunList;

    [Header("Info Gun")]
    [SerializeField] GameObject _lockGunobj;
    [SerializeField] GameObject _scrollViewObj;
    [SerializeField] List<Image> _imageLevels;
    [SerializeField] List<Button> _buttonUpdateLevels;

    [Header("Image Scrollview Gun")]
    [SerializeField] List<GameObject> _borderSelectedObj;
    [SerializeField] List<GameObject> _lockObj;
    [SerializeField] List<GameObject> _unlockObj;

    private List<Image> _imageList = new();
    private List<Button> _buttons = new();

    private int _itemCount;
    private int _currentSelected = 0;
    private const float _durationChangeAlpha = 0.3f;

    //Data
    private int _totalCoin;
    private const int maxLevel = 3;

    private List<GunStat> _gunStatDatas;
    private List<StatLevel> _gunStatLevels;

    private Dictionary<StatType, StatUI> _statUIs = new();
 
    //Save
    private SaveSystem _saveSystem;

    private void Awake()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

    private void Start()
    {
        _saveSystem = GetComponent<SaveSystem>();

        _gunStatDatas = LoadData.Instance.GetGunData().gunStats;
        _gunStatLevels = LoadData.Instance.GetGunStatData().statLevels;

        AddList();

        // Add Action Button gun
        AddListenerButtonSelectedGun();
        actionSelected += HandleSelectedGun;

        // Add Action ButttonUpdateLevel
        AddListenerButtonUpdateLevel();
        actionUpdateLevel += HandleUpdateLevel;

        InitStatDictionary();
        SetUpScrollViewGun();
    }


    private void InitStatDictionary()
    {
        _statUIs[StatType.MagSize] = new StatUI(_imageLevels[0], _textCoinUpdateLevels[0]);
        _statUIs[StatType.BulletSpeed] = new StatUI(_imageLevels[1], _textCoinUpdateLevels[1]);
        _statUIs[StatType.TimeReload] = new StatUI(_imageLevels[2], _textCoinUpdateLevels[2]);
        _statUIs[StatType.FireRate] = new StatUI(_imageLevels[3], _textCoinUpdateLevels[3]);
    }

    private void SetUpScrollViewGun()
    {
       for(int i = 0; i < _itemCount; i++)
        {
            SetAlpha(_imageList[i], 0);

            bool isUnlock = _gunStatDatas[i].unlock;
            _lockObj[i].SetActive(!isUnlock);
            _unlockObj[i].SetActive(isUnlock);
    

            if (_gunStatDatas[i].equip)
            {
                SetAlpha(_imageList[i], 1);
                _currentSelected = i;
                ChangeInfoSelected(i);
                ChangeGunSelected(i);
                ChangeUpdateScrollViewInfo(_gunStatLevels[i]);
            }    
               
        }    
       
    }

    private void AddList()
    {
        _itemCount = _borderSelectedObj.Count;
        if (_itemCount != _lockObj.Count || _itemCount != _unlockObj.Count)
        {
            Debug.Log("Data GunSelected Not Valid");
            return;
        }

        for (int i = 0; i < _itemCount; i++)
        {
            _imageList.Add(_borderSelectedObj[i].GetComponent<Image>());
            _buttons.Add(_borderSelectedObj[i].GetComponent<Button>());
        }
    }

    private void AddListenerButtonSelectedGun()
    {
        for(int i = 0; i < _itemCount; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => OnClickSelectedGun(index));
        }    
    }

    private void AddListenerButtonUpdateLevel()
    {
        int paraCount = Enum.GetValues(typeof(StatType)).Length;
        for(int i = 0; i < paraCount; i++)
        {
            StatType type = (StatType)i;
            _buttonUpdateLevels[i].onClick.AddListener(() => OnClickUpdateLevel(type));
        }    
    }

    /* 
     * Handle ClickSelectedGun
     */

    private void OnClickSelectedGun(int index)
    {
        if (index == _currentSelected) return;
        actionSelected?.Invoke(index);
    }

    private void HandleSelectedGun(int index)
    {
        ChangeBorderSelected(_imageList[index], _imageList[_currentSelected]);
        ChangeGunSelected(index);
        ChangeInfoSelected(index);
        ChangeUpdateScrollViewInfo(_gunStatLevels[index]);
        _currentSelected = index;
    }

    private void ChangeBorderSelected(Image imageSelected, Image imageCurrent)
    {
        StartCoroutine(ChangeAlpha(imageSelected, 0f, 1f));
        StartCoroutine(ChangeAlpha(imageCurrent, 1f, 0f));
    }

    private void ChangeGunSelected(int index)
    {
        _textNameGun.text = _gunStatDatas[index].nameGun;
        _imageGun.sprite = _spriteGunList[index];
    }


    private void ChangeInfoSelected(int index)
    {
        bool isUnlock = _gunStatDatas[index].unlock;
        _scrollViewObj.SetActive(isUnlock);
        _lockGunobj.SetActive(!isUnlock);

        if (isUnlock)
            _textCoinBuyGun.text = _gunStatDatas[index].priceCoin.ToString();
    }


    private IEnumerator ChangeAlpha(Image image, float start, float end)
    {
        float timer = 0f;
        while (timer <= _durationChangeAlpha)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(start, end, timer / _durationChangeAlpha);
            SetAlpha(image, alpha);
            yield return null;
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }


    /* 
     * Handle ClickUpdate
     */

    private void OnClickUpdateLevel(StatType type)
    {      
        actionUpdateLevel?.Invoke(type);
    }    

    private void HandleUpdateLevel(StatType type)
    {
        bool success = TryUpradeStat(type);
        if (success)
        {
            List<DataLevel> levels = GetStatLevels(type);
            UpdateStatUI(type, levels);
            _saveSystem.SaveData();
            PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _totalCoin);
        }
    }    

    private bool TryUpradeStat(StatType type)
    {
        var statUI = _statUIs[type];
        int level = statUI.Level;
        int price = statUI.Price;
        List<DataLevel> levels = GetStatLevels(type);

        if (level >= maxLevel || _totalCoin < price) return false;

        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].level != level) continue;
            levels[i + 1].unlock = true;
            _totalCoin -= price;
            return true;
        }

        return false;
    }

    private List<DataLevel> GetStatLevels(StatType type)
    {
        var statLevel = _gunStatLevels[_currentSelected];
        return type switch
        {
            StatType.MagSize => statLevel.magSize,
            StatType.BulletSpeed => statLevel.bulletSpeed,
            StatType.TimeReload => statLevel.timeReload,
            StatType.FireRate => statLevel.fireRate,
            _ => null,
        };
    }


    private void ChangeUpdateScrollViewInfo(StatLevel statLevel)
    {
        UpdateStatUI(StatType.MagSize, statLevel.magSize);
        UpdateStatUI(StatType.BulletSpeed, statLevel.bulletSpeed);
        UpdateStatUI(StatType.TimeReload, statLevel.timeReload);
        UpdateStatUI(StatType.FireRate, statLevel.fireRate);
    }

    private void UpdateStatUI(StatType type, List<DataLevel> levels)
    {
        var statUI = _statUIs[type];
        for(int i = maxLevel - 1; i >= 0; i--)
        {
            if (!levels[i].unlock) continue;

            statUI.Level = levels[i].level;
            statUI.Image.fillAmount = (float)statUI.Level/maxLevel;

            if(statUI.Level < maxLevel)
            {
                statUI.Price = levels[i + 1].price;
                statUI.TextCoin.text = statUI.Price.ToString();
            }
            break;
        }
    }


}

public enum StatType
{
    MagSize,
    BulletSpeed,
    TimeReload,
    FireRate
}

[System.Serializable]
public class StatUI
{
    public Image Image;
    public TextMeshProUGUI TextCoin;
    public int Level;
    public int Price;

    public StatUI(Image image, TextMeshProUGUI textCoin)
    {
        Image = image;
        TextCoin = textCoin;
    }
}