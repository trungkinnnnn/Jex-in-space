using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ScreenHome : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] Image _logoStuido;
    private const string _para_Raius_Name = "_Radius";
    private const string _SCENE_NAME = "InGameScreen";

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        SetAlpha(0f);
        _material.SetFloat(_para_Raius_Name, 0f);

        yield return StartCoroutine(Wait(2f));
        _logoStuido.DOFade(1f, 3f).SetEase(Ease.OutQuad);

        yield return StartCoroutine(Wait(2f));
        _logoStuido.DOFade(0f, 1f).SetEase(Ease.OutQuad);

        yield return StartCoroutine(Wait(1f));
        yield return StartCoroutine(LoadingScreen.Instance.Hide());
    }

    public void ActionNextSceneInGame()
    {
        AudioSystem.Instance.PlayAudioClick();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME);
    }    

    private void SetAlpha(float alpha)
    {
        Color color = _logoStuido.color;
        color.a = alpha;
        _logoStuido.color = color;
    }
    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

}
