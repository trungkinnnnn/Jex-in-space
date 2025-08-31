
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopModuleSceenUI : MonoBehaviour
{
    [Header("data")]
    [SerializeField] SkillData _skillData;

    [Header("TextUI")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    [Header("Button")]
    [SerializeField] List<Button> _buttonBuyModules;

    [Header("Text")]    
    [SerializeField] List<TextMeshProUGUI> _textBuyModules;
    [SerializeField] List<TextMeshProUGUI> _textNumberModules;
    private List<int> _counts = new List<int> {1, 1};


    private Dictionary<TypeModule, int> _dicModules = new();
    private ShopGunScreenUI _shopGunScreenUI;
    private int _totalCoin;
    private void Start()
    {
        _shopGunScreenUI = GetComponent<ShopGunScreenUI>();
        SetUpTotalCoin();
        SetUpText();
        AddModuleType();
        AddActionButtonBuys();
    }

    private void SetUpTotalCoin()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

    public void SetTextCoin(int coin)
    {
        _textTotalCoin.text = coin.ToString();
    }

    private void SetUpText()
    {
        for(int i = 0; i < _textBuyModules.Count; i++)
        {
            _textBuyModules[i].text = _skillData.price[i].ToString();
            _textNumberModules[i].text = _counts[i].ToString();
        }    
    }    

    private void AddModuleType()
    {
        for(int i = 0; i < _textBuyModules.Count; i++)
        {
            _dicModules.Add((TypeModule)i, _skillData.price[i]);
        }    
    }    

    private void AddActionButtonBuys()
    {
        for(int i = 0; i< _buttonBuyModules.Count; i++)
        {
            TypeModule type = (TypeModule)i;
            _buttonBuyModules[i].onClick.AddListener(() => ActionBuyModule(type));
        }    
    }    



    private void ActionBuyModule(TypeModule type)
    {
        int index = (int)type;
        int price = _dicModules[type];
        if(_totalCoin - price > 0)
        {
            BuyModule(index);
            _totalCoin -= price;
            _shopGunScreenUI.SetTextCoin(_totalCoin);
            SaveData();
        }    
    }    

    private void BuyModule(int index)
    {
        _counts[index] += 1;
        HandleUpdateTextNumberModule(index, _counts[index]);
    }

    private void SaveData()
    { 
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _totalCoin);
        PlayerPrefs.Save();
    }

    private void HandleUpdateTextNumberModule(int index, int count) => _textNumberModules[index].text = count.ToString();


}

public enum TypeModule
{
    shoot,
    shockwave,
}


