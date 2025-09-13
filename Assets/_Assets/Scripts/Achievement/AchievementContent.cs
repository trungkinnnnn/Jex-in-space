using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchievementContent : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] GameObject _normal;
    [SerializeField] GameObject _secret;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textDescription;
    [SerializeField] TextMeshProUGUI _textTarget;
    [SerializeField] TextMeshProUGUI _textCoinSuccess;

    [Header("Button")]
    [SerializeField] Button _completedBtn;
    [SerializeField] GameObject _notCompletedBtn;
    [SerializeField] GameObject _claimedBtn;

    private TextMeshProUGUI _textTotalCoin;
    private Achievement _achi;
    private int _totalCoin;
    private void OnEnable()
    {
        if (_achi == null) return;
        SetUp(_achi);
        GetTotalCoin();
    }

    public void Init(Achievement data, int countDestroyAst, TextMeshProUGUI text)
    {
        _textTotalCoin = text;

        _achi = data;
        _achi.min = countDestroyAst;

        SetUp(data);
    }

    private void Start()
    {
        _completedBtn.onClick.AddListener(() => HandleActionSuccess());
        _textCoinSuccess.text = _achi.coin.ToString();

        SetText(_achi);
    }

    private void SetUp(Achievement data)
    {
        SetType(data);
        SetText(data);
        SetButton(data);
    }

    private void SetType(Achievement data)
    {
        _normal.gameObject.SetActive(!data.secret);
        _secret.gameObject.SetActive(data.secret);
    }

    private void SetText(Achievement data)
    {
        _textDescription.text = data.description;
        if (data.completed)
        {
            data.min = data.max;
            
        }
        _textTarget.text = data.min + "/" + data.max;
    }

    private void SetButton(Achievement data)
    {
        bool close = (data.min < data.max);
        _completedBtn.gameObject.SetActive(data.completed && !data.claimed);
        _notCompletedBtn.gameObject.SetActive(close);
        _claimedBtn.gameObject.SetActive(data.claimed && data.completed);
    }

    private void HandleActionSuccess()
    {
        _achi.claimed = true;   
        SetButton(_achi);

        _totalCoin += _achi.coin;
        SaveDataTotalCoin();
        AchievementManager.Instace.SaveAchievementData();
    }    

    private void GetTotalCoin()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN, 0);
        _textTotalCoin.text = _totalCoin.ToString();
    }    

    private void SaveDataTotalCoin()
    {
        _textTotalCoin.text = _totalCoin.ToString();
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _totalCoin);
        PlayerPrefs.Save();
    }    

}
