using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopGunScreenUI : MonoBehaviour
{

    public Action<int> actionSelected;

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

    [Header("Image Scrollview Gun")]
    [SerializeField] List<GameObject> _borderSelectedObj;
    [SerializeField] List<GameObject> _lockObj;
    [SerializeField] List<GameObject> _unlockObj;

    private List<Image> _imageList = new List<Image>();
    private List<Button> _buttons = new List<Button>();

    private int _itemCount;
    private int _currentSelected = 0;
    private float _durationChangeAlpha = 0.3f;
    private float _alphaMax = 1f;
    private float _alphaMin = 0f;

    //Check StatLevels
    private bool _magSize = false;  
    private bool _bulletSpeed = false;
    private bool _timeReload = false;   
    private bool _fireRate = false;

    //Data
    private int _totalCoin;
    private int maxLevel = 3;
    private List<GunStat> _gunStatDatas;
    private List<StatLevel> _gunStatLevels;

    private void Awake()
    {
        AddList();
        AddListener();
        actionSelected += HandleSelectedGun;

        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

    private void Start()
    {
        _gunStatDatas = LoadData.Instance.GetGunData().gunStats;
        _gunStatLevels = LoadData.Instance.GetGunStatData().statLevels;
        Debug.Log("Number Gun: " + _gunStatDatas.Count);
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

    private void AddListener()
    {
        for(int i = 0; i< _itemCount; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => OnClick(index));
        }    

    }

    private void OnClick(int index)
    {
        Debug.Log("Gun :" +  index + "Current : " + _currentSelected);
        if (index == _currentSelected) return;
        actionSelected?.Invoke(index);
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
        StartCoroutine(ChangeAlpha(imageSelected, _alphaMin, _alphaMax));
        StartCoroutine(ChangeAlpha(imageCurrent, _alphaMax, _alphaMin));
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
                    textCoin[0].text = statLevel.magSize[i + 1].price.ToString();
                }
                _magSize = true;    
            }

            if (statLevel.bulletSpeed[i].unlock && !_bulletSpeed)
            {
                level = statLevel.bulletSpeed[i].level;
                images[1].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    textCoin[1].text = statLevel.bulletSpeed[i + 1].price.ToString();
                }
                _bulletSpeed = true;
            }

            if (statLevel.timeReload[i].unlock && !_timeReload)
            {
                level = statLevel.timeReload[i].level;
                images[2].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    textCoin[2].text = statLevel.timeReload[i + 1].price.ToString();
                }
                _timeReload = true;
            }

            if (statLevel.fireRate[i].unlock && !_fireRate)
            {
                level = statLevel.fireRate[i].level;
                images[3].fillAmount = (float)level / maxLevel;
                if (level < maxLevel)
                {
                    textCoin[3].text = statLevel.fireRate[i + 1].price.ToString();
                }
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
