using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private List<int> _numberSkill = new();

    private void Awake()
    {
        SetUpNumberSkill();
    }

    private void SetUpNumberSkill()
    {
        string number = PlayerPrefs.GetString(DataPlayerPrefs.para_SKILL, "");
        if (number == "")
        {
            PlayerPrefs.SetString(DataPlayerPrefs.para_SKILL, "1,1");
            PlayerPrefs.Save();
            number = "1,1";
        }
        AddListNumberSkill(number);
    }

    private void AddListNumberSkill(string number)
    {
        var list = number.Split(',');
        foreach (var i in list)
        {
            _numberSkill.Add((int)TryPaseInt(i));
        } 
    }

    private int? TryPaseInt(string str) => int.TryParse(str, out var result) ? result : null;

    public List<int> GetListNumberSkill() => _numberSkill;  

    public void SetNumberIndexList(int index, int count)
    {
        _numberSkill[index] = count;
        SaveData();
    }

    private void SaveData()
    {
        string _number = _numberSkill[0].ToString();
        for(int i = 1; i < _numberSkill.Count; i++)
        {
            _number += "," + _numberSkill[i].ToString();
        }

        PlayerPrefs.SetString(DataPlayerPrefs.para_SKILL, _number);
        PlayerPrefs.Save();
    }

}
