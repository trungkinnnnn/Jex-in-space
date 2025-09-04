
using UnityEngine;

public class AudioBGM_InGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AudioBGMManager.Instance.WaitStartMusicInGame());
    }

}
