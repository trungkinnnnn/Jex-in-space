using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopGunScreenUI : MonoBehaviour
{

    public Action<int> actionSelected;
    public Action<int> actionUpdateLevel;

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

    private List<Image> _imageList = new List<Image>();
    private List<Button> _buttons = new List<Button>();

    private int _itemCount;
    private int _currentSelected = 0;
    private float _durationChangeAlpha = 0.3f;

    //Check StatLevels
    private bool _magSize = false;  
    private bool _bulletSpeed = false;
    private bool _timeReload = false;   
    private bool _fireRate = false;

    private bool _updateLevelConfirm = false;

    //Data
    private int _totalCoin;
    private int maxLevel = 3;

    private List<GunStat> _gunStatDatas;
    private List<StatLevel> _gunStatLevels;
    private GunStatSelected _gunStatSelected = new GunStatSelected();

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

        SetUpScrollViewGun();
    }

    private void SetUpScrollViewGun()
    {
       for(int i = 0; i < _itemCount; i++)
        {
            SetAlpha(_imageList[i], 0);
            if (_gunStatDatas[i].unlock)
            {
                _lockObj[i].SetActive(false);
                _unlockObj[i].SetActive(true);
            }
            else
            {
                _lockObj[i].SetActive(true);
                _unlockObj[i].SetActive(false);
            }

            if (_gunStatDatas[i].equip)
            {
                SetAlpha(_imageList[i], 1);
                _currentSelected = i;
                ChangeInfoSelected(i);
                ChangeGunSelected(i);
                ChangeUpdateScrollViewInfo(_imageLevels, _textCoinUpdateLevels, _gunStatLevels[i]);
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
            Image image = _borderSelectedObj[i].GetComponent<Image>();
            Button button = _borderSelectedObj[i].GetComponent<Button>();
            _imageList.Add(image);
            _buttons.Add(button);
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
        int paraPublic = _gunStatLevels[0].GetType().GetFields().Length - 1;
        for(int i = 0; i < paraPublic; i++)
        {
            int index = i;
            _buttonUpdateLevels[i].onClick.AddListener(() => OnClickUpdateLevel(index));
        }    
    }

    private void OnClickSelectedGun(int index)
    {
        //Debug.Log("Gun :" +  index + "Current : " + _currentSelected);
        if (index == _currentSelected) return;
        actionSelected?.Invoke(index);
    }    

    private void OnClickUpdateLevel(int index)
    {      
        actionUpdateLevel?.Invoke(index);
    }    

    private void HandleUpdateLevel(int index)
    {
        //Debug.Log("Number Update : " + index);
        switch(index)
        {
            case 0:
                {
                    int level = _gunStatSelected.magSizeLevel;
                    int price = _gunStatSelected.magSizePrice;
                    if (level >= maxLevel) break;
                    if (_totalCoin < price) break;
                    UpdateLevel(_gunStatLevels[_currentSelected].magSize, level, price);
                    _updateLevelConfirm = true;
                    break;
                }
            case 1:
                {
                    int level = _gunStatSelected.bulletSpeedLevel;
                    int price = _gunStatSelected.bulletSpeedPrice;
                    if (level >= maxLevel) break;
                    if (_totalCoin < price) break;
                    UpdateLevel(_gunStatLevels[_currentSelected].bulletSpeed, level, price);
                    _updateLevelConfirm = true;
                    break;
                }
            case 2:
                {
                    int level = _gunStatSelected.timeReloadLevel;
                    int price = _gunStatSelected.timeReloadPrice;
                    if (level >= maxLevel) break;
                    if (_totalCoin < price) break;
                    UpdateLevel(_gunStatLevels[_currentSelected].timeReload, level, price);
                    _updateLevelConfirm = true;
                    break;
                }
            case 3:
                {
                    int level = _gunStatSelected.fireRateLevel;
                    int price = _gunStatSelected.fireRatePrice;
                    if (level >= maxLevel) break;
                    if (_totalCoin < price) break;
                    UpdateLevel(_gunStatLevels[_currentSelected].fireRate, level, price);
                    _updateLevelConfirm = true;
                    break;
                }
            default:
                break;
        }  
        
        if (_updateLevelConfirm)
            ChangeUpdateScrollViewInfo(_imageLevels, _textCoinUpdateLevels, _gunStatLevels[_currentSelected]);
            _saveSystem.SaveData();
            PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _totalCoin);
            _updateLevelConfirm = false;
    }    

    private void UpdateLevel(List<DataLevel> dataLevels, int currentLevel, int price)
    {
        for(int i = 0; i < dataLevels.Count;i++)
        {
            if(dataLevels[i].level != currentLevel)continue;
            dataLevels[i + 1].unlock = true;
            _totalCoin -= price;
            break;
        }
        //Debug.Log("unlock Level" + _gunStatLevels[_currentSelected].magSize[2].unlock);
    }

    private void HandleSelectedGun(int index)
    {
        for (int i = 0; i < _itemCount; i++)
        {
            if (index != i) continue;
            ChangeBorderSelected(_imageList[index], _imageList[_currentSelected]);
            ChangeGunSelected(index);
            ChangeInfoSelected(index);
            ChangeUpdateScrollViewInfo(_imageLevels, _textCoinUpdateLevels, _gunStatLevels[index]);
            _currentSelected = i;
            break;
        }
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
        if(_gunStatDatas[index].unlock)
        {
            _scrollViewObj.SetActive(true);
            _lockGunobj.SetActive(false);
           
        }else
        {
            _scrollViewObj.SetActive(false);
            _lockGunobj.SetActive(true);
            _textCoinBuyGun.text = _gunStatDatas[index].priceCoin.ToString();
        }
    }

    private void ChangeUpdateScrollViewInfo(List<Image> images, List<TextMeshProUGUI> textCoin, StatLevel statLevel)
    {
       for(int i = maxLevel - 1; i >= 0; i--)
        {
            int level = 1;
            if (statLevel.magSize[i].unlock && !_magSize)
            {
                level = statLevel.magSize[i].level;
                images[0].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    // hiển thị giá của level kế tiếp
                    int price = statLevel.magSize[i + 1].price;
                    _gunStatSelected.magSizePrice = price;
                    textCoin[0].text = price.ToString();
                }
                _gunStatSelected.magSizeLevel = level;
                _magSize = true;    
            }

            if (statLevel.bulletSpeed[i].unlock && !_bulletSpeed)
            {
                level = statLevel.bulletSpeed[i].level;
                images[1].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    int price = statLevel.bulletSpeed[i + 1].price;
                    _gunStatSelected.bulletSpeedPrice = price;
                    textCoin[1].text = price.ToString();
                }
                _gunStatSelected.bulletSpeedLevel = level;
                _bulletSpeed = true;
            }

            if (statLevel.timeReload[i].unlock && !_timeReload)
            {
                level = statLevel.timeReload[i].level;
                images[2].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    int price = statLevel.timeReload[i + 1].price;
                    _gunStatSelected.timeReloadPrice = price;
                    textCoin[2].text = price.ToString();
                }
                _gunStatSelected.timeReloadLevel = level;
                _timeReload = true;
            }

            if (statLevel.fireRate[i].unlock && !_fireRate)
            {
                level = statLevel.fireRate[i].level;
                images[3].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    int price = statLevel.fireRate[i + 1].price;
                    _gunStatSelected.fireRatePrice = price;
                    textCoin[3].text = price.ToString();
                }
                _gunStatSelected.fireRateLevel = level;
                _fireRate = true;
            }
        }

        _magSize = false;
        _bulletSpeed = false;
        _timeReload = false;
        _fireRate = false;

    }

    private IEnumerator ChangeAlpha(Image image, float start, float end)
    {
        float timer = 0f;
        while(timer <= _durationChangeAlpha)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(start, end, timer/_durationChangeAlpha);
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

}

[System.Serializable]
public class GunStatSelected
{
    public int magSizeLevel;
    public int magSizePrice;
    public int bulletSpeedLevel;
    public int bulletSpeedPrice;
    public int timeReloadLevel;
    public int timeReloadPrice;
    public int fireRateLevel;
    public int fireRatePrice;
}