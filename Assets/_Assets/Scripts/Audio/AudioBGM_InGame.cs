using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBGM_InGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AudioManager.Instance.WaitStartMusicInGame());
    }

}
