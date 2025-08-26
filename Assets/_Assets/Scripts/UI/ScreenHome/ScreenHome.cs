using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHome : MonoBehaviour
{
    private const string _SCENE_NAME = "InGameScreen";
  

    public void ActionNextSceneInGame()
    {
        AudioSystem.Instance.PlayAudioClick();
        LoadingScene.Instance.LoadingScence(_SCENE_NAME);
    }    

}
