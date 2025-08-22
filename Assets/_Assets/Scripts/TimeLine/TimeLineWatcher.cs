
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class TimeLineWatcher : MonoBehaviour
{
    [SerializeField] PlayableDirector _timeLine1;
    [SerializeField] PlayableDirector _timeLine2;
    [SerializeField] Camera _camera;

    private bool _resetScene = false;
    private Animator _animator;

    public float _timeDuration = 3f;
    private void Start()
    {
        _animator = _camera.GetComponent<Animator>();

        if (_timeLine1 != null)
        {
            _timeLine1.gameObject.SetActive(true);
            _timeLine1.time = 0;
            _timeLine1.Play();
        }    

        if (_timeLine2 != null)
        {   
            _timeLine2.gameObject.SetActive(false);
            _timeLine2.Stop();
            _timeLine2.time = 0;
        }
       
    }

    private void Update()
    {
        
        if (!_resetScene) return;
        if (_timeLine2 != null && 
            _timeLine2.state == PlayState.Playing && 
            _timeLine2.time >= _timeLine2.duration)
        {
            OnTimeFinished(_timeLine2);
        }    
    }

    private void OnEnable()
    {  
        DieScreenUI.Reset += ActionReset;
    }

    private void OnDestroy()
    {
       
        DieScreenUI.Reset -= ActionReset;
    }
    private void OnTimeFinished(PlayableDirector director)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        ////Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }    

    private void ActionReset()
    {
        Debug.Log("Time line run");
        _timeLine2.gameObject.SetActive(true);
        _timeLine2.time = 0;
        _timeLine2.Play();
        _resetScene = true;

        _animator.enabled = true;

        Time.timeScale = 1;
        //Time.fixedDeltaTime = 0.02f;
    }  
}
