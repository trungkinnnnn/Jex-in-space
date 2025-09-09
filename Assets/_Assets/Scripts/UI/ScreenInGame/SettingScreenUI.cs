
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingScreenUI : MonoBehaviour
{
    // ScreenDie
    public static Action Die;

    [Header("ButtonOn")]
    [SerializeField] List<Button> _buttonOn;

    [Header("ButtonOff")]
    [SerializeField] List<Button> _buttonOff;

    [Header("ButtonDie")]
    [SerializeField] Button _buttonDie;

    private string _setting;
    private List<bool> _isActive;

    private const string _SCENE_NAME = "HomeScreen";
   
    private void Start()
    {
        AddListenerButton();
        GetData();
        SetUpButton();
    }

    private void GetData()
    {
        _setting = PlayerPrefs.GetString(DataPlayerPrefs.para_Setting,"");
        if (_setting == "" || _setting == null)
        {
            _isActive = new List<bool> { true, true, false };
            return;
        }       
        SplitData(_setting);
    }    

    private void SplitData(string value)
    {
        if (_isActive == null)
            _isActive = new List<bool>();

        var split = value.Split(',');
        for(int i = 0; i < split.Length; i++)
        {
            bool v = (bool)TryPaserBool(split[i]);
            _isActive.Add(v);   
        }
    }

    static bool? TryPaserBool(string str) => str.Trim().ToLower() == "true"; 

    private void AddListenerButton()
    {
        if(_buttonOn.Count != _buttonOff.Count)
        {
            Debug.Log("Not Valid");
            return;
        }    

        for(int i = 0;i < _buttonOn.Count;i++)
        {
            int index = i;
            _buttonOn[i].onClick.AddListener(() => ActionSetting(index));
            _buttonOff[i].onClick.AddListener(() => ActionSetting(index));
        }  
    }    

    private void SetUpButton()
    {
        for(int i = 0; i < _isActive.Count;i++)
        {
            SetActive(i, _isActive[i]);
        }

        _buttonDie.onClick.AddListener(() => ActionDie());
    }    

    public void ActionBackMenu()
    {
        AudioSystem.Instance.PlayAudioClick();

        PausePhysic2D.Instance.ResumeGame();
        AudioBGMManager.Instance.PlayMenuBGM();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME);
    }

    public void ActionDie()
    {
        _buttonDie.gameObject.SetActive(false);
        AudioSystem.Instance.PlayAudioClick();
        AudioSystem.Instance.PlayAudioGameOver();

        AdsManager.Instance.LoadRewardedAd();

        Die?.Invoke();
    }

    public void ActionSetting(int index)
    {
        SetActive(index, !_isActive[index]);
        _isActive[index] = !_isActive[index];

        switch(index)
        {
            case 0:
                AudioSFX.Instance.SetActive(_isActive[index]);
                break;
            case 1:
                AudioBGMManager.Instance.SetActive(_isActive[index]);
                break;
            case 2:
                CameraShake.Instance.SetActive(_isActive[index]);
                break;
            default:
                break;
        }    
        

        SaveSetting();
    }    

    private void SetActive(int index, bool isActive)
    {
        _buttonOn[index].gameObject.SetActive(isActive);
        _buttonOff[index].gameObject.SetActive(!isActive);
    }

    private void SaveSetting()
    {
        string save = $"{_isActive[0]}";
        for (int i = 1; i < _isActive.Count; i++)
        {
            save += "," + $"{_isActive[i]}";
        }            
        PlayerPrefs.SetString(DataPlayerPrefs.para_Setting, save);
        PlayerPrefs.Save();
    }
       
}
