using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textCoin;
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] List<TextMeshProUGUI> _textSkills;

    [Header("Script")]
    [SerializeField] HUDController _hudScreen;
    [SerializeField] SkillController _skillController;

    private void OnEnable()
    {
        HandleUpdateCoin();
        HandleUpdateScore();
        SetTextSkillNumber(_skillController.GetListNumberSkill());
    }

    private void SetTextSkillNumber(List<int> lists)
    {
        for(int i = 0; i < lists.Count; i++)
        {
            _textSkills[i].text = lists[i].ToString();
        }
    }
    
    private void HandleUpdateCoin() => _textCoin.text = _hudScreen.GetCoin().ToString();
    private void HandleUpdateScore() => _textScore.text = _hudScreen.GetScore().ToString();
}
