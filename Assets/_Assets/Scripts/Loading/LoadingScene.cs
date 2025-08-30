using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene Instance;

    private LoadingScreen _loadingScreen;
    private float _timeWait = 0.5f;
    
    private bool _hideShow = false;
    private bool _isloading = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        _loadingScreen = LoadingScreen.Instance;
        if (_loadingScreen == null) Debug.Log("LoadingScreen NUll");
    }

    public void LoadingScence(string nameScene)
    {
        if (_isloading) return;
        _isloading = true;
        _hideShow = false ;
        StartCoroutine(LoadSceneAsync(nameScene));
    }    

    private IEnumerator LoadSceneAsync(string nameScene)
    {
        if (_loadingScreen != null)
            yield return StartCoroutine(_loadingScreen.ShowCircle());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && !_hideShow)
            {
                
                asyncLoad.allowSceneActivation = true;
                yield return new WaitForSeconds(_timeWait);

                _loadingScreen = LoadingScreen.Instance;
                if (_loadingScreen != null) yield return StartCoroutine(_loadingScreen.HideCircle());
                _hideShow = true;
                _isloading = false;
            }
            yield return null;
        }    

    }    


}
