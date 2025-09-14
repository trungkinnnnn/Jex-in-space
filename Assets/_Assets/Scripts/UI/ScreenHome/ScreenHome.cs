using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class ScreenHome : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] Image _logoStuido;
    private static string _para_Raius_Name = "_Radius";
    private static string _para_Alpha_Name = "_Alpha";

    private const string _SCENE_NAME_TUTORIAL = "Tutorial";
    private const string _SCENE_NAME_INGAME = "InGameScreen";

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        SetAlpha(0f);
        _material.SetFloat(_para_Raius_Name, 0f);
        _material.SetFloat(_para_Alpha_Name, 1f);

        yield return new WaitForSeconds(2f);
        _logoStuido.DOFade(1f, 3f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(2f);
        _logoStuido.DOFade(0f, 1f).SetEase(Ease.OutQuad);

        CheckFirstPlay();

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(LoadingScreen.Instance.HideAlpha(1f));
    }

    public void ActionNextSceneInGame()
    {
        AudioSystem.Instance.PlayAudioClick();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME_INGAME);
    }    

    private void SetAlpha(float alpha)
    {
        Color color = _logoStuido.color;
        color.a = alpha;
        _logoStuido.color = color;
    }

    private void CheckFirstPlay()
    {
        int first = PlayerPrefs.GetInt(DataPlayerPrefs.fistPlay, 0);
        if (first == 0)
            SceneManager.LoadScene(_SCENE_NAME_TUTORIAL);
    }

}
