
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineController : MonoBehaviour
{
   
    private PlayableDirector _playableDirector;

    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        _playableDirector.Play();
    }

}
