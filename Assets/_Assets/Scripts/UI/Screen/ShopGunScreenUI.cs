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
    [Header("Border")]
    [SerializeField] List<GameObject> _borderSelectedObj;

    [Header("Lock")]
    [SerializeField] List<Image> _imageLock;

    [Header("Unlock")]
    [SerializeField] List<Image> _imageUnlock;

    [Header("TextUI")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    private List<Image> _imageList = new List<Image>();
    private List<Button> _buttons = new List<Button>();

    private int _itemCount;
    private int _currentSelected = 0;
    private float _durationChangeAlpha = 0.3f;
    private float _alphaMax = 1f;
    private float _alphaMin = 0f;

    //Data
    private int _totalCoin;

    private void Start()   
    {
        SetUp();
        AddListener();
        actionSelected += HandleSelectedGun;

        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

    private void SetUp()
    {
        _itemCount = _borderSelectedObj.Count;
        if (_itemCount != _imageLock.Count || _itemCount != _imageUnlock.Count)
        {
            Debug.Log("Data GunSelected Not Valid");
            return;
        }

        for(int i = 0; i < _itemCount; i++)
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
            Image imageSelected = _imageList[index];
            Image imageCurrent = _imageList[_currentSelected];
            _currentSelected = i;
            OnSelected(imageSelected, imageCurrent);
            break;
        }
    }

    private void OnSelected(Image imageSelected, Image imageCurrent)
    {
        StartCoroutine(ChangeAlpha(imageSelected, _alphaMin, _alphaMax));
        StartCoroutine(ChangeAlpha(imageCurrent, _alphaMax, _alphaMin));
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
