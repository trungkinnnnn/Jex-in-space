
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopModuleSceenUI : MonoBehaviour
{
    [Header("data")]
    [SerializeField] SkillDataPrice _skillData;

    [Header("TextUI")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    [Header("Button")]
    [SerializeField] List<Button> _buttonBuyModules;

    [Header("Text")]    
    [SerializeField] List<TextMeshProUGUI> _textBuyModules;
    [SerializeField] List<TextMeshProUGUI> _textNumberSkills;

    [Header("Script")]
    [SerializeField] SkillController _skillController;
    private List<int> _countSkills;

    private Dictionary<TypeModule, int> _dicModules = new();
    private int _totalCoin;

    private void OnEnable()
    {
        SetTextCoin();
        SetTextNumberSkill(_skillController.GetListNumberSkill());
    }

    private void Start()
    {
        SetUpText();
        AddModuleType();
        AddActionButtonBuys();
    }

    private void SetTextCoin()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

    public void SetTextCoin(int totalCoin)
    {
        _textTotalCoin.text = totalCoin.ToString();
    }

    private void SetUpText()
    {
        for(int i = 0; i < _textBuyModules.Count; i++)
        {
            _textBuyModules[i].text = _skillData.price[i].ToString();
        }    
    }

    private void SetTextNumberSkill(List<int> numberSkillList)
    {
        _countSkills = numberSkillList;
        for (int i = 0; i < numberSkillList.Count; i++)
        {
            _textNumberSkills[i].text = numberSkillList[i].ToString();
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
            SetTextCoin(_totalCoin -= price);
            SaveData();
        }    
    }    

    private void BuyModule(int index)
    {
        _skillController.SetNumberIndexList(index, _countSkills[index] += 1);
        HandleUpdateTextNumberModule(index, _countSkills[index]);
    }

    private void HandleUpdateTextNumberModule(int index, int count) => _textNumberSkills[index].text = count.ToString();

    private void SaveData()
    { 
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _totalCoin);
        PlayerPrefs.Save();
    }

    


}

public enum TypeModule
{
    shockwave,
    shoot,
}
    
    


