using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    private const string _SCENE_NAME = "HomeScreen";
    private PausePhysic2D _pausePhysic2D;
    private void Start()
    {
        _pausePhysic2D = GetComponent<PausePhysic2D>();
    }

    public void ActionBackMenu()
    {
        _pausePhysic2D.ResumeGame();
        AudioManager.Instance.PlayMenuBGM();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME);
    }    

}
