using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Button")]
    [SerializeField] Button _normalBtn;
    [SerializeField] GameObject _closeBtn;
    [SerializeField] GameObject _successBtn;

    public void Init(Achievement data)
    {
        SetUp(data);
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
        _secret.gameObject.SetActive(data.secret && !data.success);
    }

    private void SetText(Achievement data)
    {
        _textDescription.text = data.description;
        _textTarget.text = data.min + "/" + data.max;
    }

    private void SetButton(Achievement data)
    {
        bool close = (data.min != data.max);
        _normalBtn.gameObject.SetActive(!close && !data.success);
        _closeBtn.gameObject.SetActive(close);
        _successBtn.gameObject.SetActive(data.success);
    }

}
