using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    //Action
    public Action<int> OnTabSelected;

    [Header("Tab")]
    [SerializeField] GameObject _tabDie;

    [Header("Tabs")]
    [SerializeField] List<GameObject> _tabObjs;

    [Header("ButtonON")]
    [SerializeField] List<GameObject> _buttonONs;

    [Header("ButtonOFF")]
    [SerializeField] List<Button> _buttonOFFs;

    private GameObject _tabPause;
    private int _tabCount;
    private int _currentSeleted = 0;

    private void Start()
    {
        CheckValid();

        _tabDie.SetActive(false);

        _tabPause = _tabObjs[_currentSeleted];
        _tabCount = _tabObjs.Count;
    
        AddListenerButtonOFF();
        OnTabSelected += HandleTabSelected;

        SetObj(_tabObjs[_currentSeleted], _buttonONs[_currentSeleted], true);
    }

    private void CheckValid()
    {
        if (_tabObjs.Count != _buttonOFFs.Count && _tabObjs.Count != _buttonONs.Count)
        {
            Debug.Log("Tab miss button");
            return;
        }
    }

    private void AddListenerButtonOFF()
    {
        for(int i = 0;  i < _tabCount; i++)
        {
            int tabIndex = i;
            _buttonOFFs[i].onClick.AddListener(() => OnButtonClicked(tabIndex));
        }
    }

    private void OnButtonClicked(int tabIndex)
    {
        if (tabIndex == _currentSeleted) return;
        Debug.Log(tabIndex);
        OnTabSelected?.Invoke(tabIndex);
    }

    private void HandleTabSelected(int tabIndex)
    {
        for (int i = 0; i < _tabObjs.Count; i++)
        {
            if (i != tabIndex) continue;
            HandleTableSelect(tabIndex);
            break;
        }    
    }    

    private void HandleTableSelect(int tabIndex)
    {
        
        SetObj(_tabObjs[tabIndex], _buttonONs[tabIndex], true);
        SetObj(_tabObjs[_currentSeleted], _buttonONs[_currentSeleted], false);
        _currentSeleted = tabIndex;
    }    

    private void SetObj(GameObject objTab, GameObject button, bool value)
    {
        objTab.SetActive(value);
        button.SetActive(value);
    }


    private void OnEnable()
    {
        RegisterEvents();
    }
      
    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        PlayerHealth.Die += HandleActionOnTabDie;
        GunController.Die += HandleActionOnTabDie;
    }   
    
    private void UnregisterEvents()
    {
        PlayerHealth.Die -= HandleActionOnTabDie;
        GunController.Die -= HandleActionOnTabDie;
    }    

    private void HandleActionOnTabDie()
    {
        _tabObjs[_currentSeleted].SetActive(false);
        _tabObjs[0] = _tabDie;
        _tabDie.SetActive(true);
    }

}
