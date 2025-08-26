
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreenUI : MonoBehaviour
{
    public static Action Die;

    private const string _SCENE_NAME = "HomeScreen";
    private PausePhysic2D _pausePhysic2D;
    private void Start()
    {
        _pausePhysic2D = GetComponent<PausePhysic2D>();
    }

    public void ActionBackMenu()
    {
        AudioSystem.Instance.PlayAudioClick();

        _pausePhysic2D.ResumeGame();
        AudioBGMManager.Instance.PlayMenuBGM();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME);
    }

    public void ActionDie()
    {
        AudioSystem.Instance.PlayAudioClick();
        AudioSystem.Instance.PlayAudioGameOver();
        Die?.Invoke();
    }
}
