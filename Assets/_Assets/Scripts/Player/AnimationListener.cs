
using UnityEngine;

public class AnimationListener : MonoBehaviour
{

    private static readonly int HashAnnoyed = Animator.StringToHash("isAnnoyed");
    private static readonly int HashHurt = Animator.StringToHash("isHurt");
    private static readonly int HashEat = Animator.StringToHash("isEat");
    private static readonly int HashEat2 = Animator.StringToHash("isEat2");
    private static readonly int HashDie = Animator.StringToHash("isDie");

    private float _timeDelaySate = 0.3f;
    private float _lastTimeState = 0f;   

    private bool _eatToggle = false;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        BounceOffWall.OnAnnoyed += Annoyed;
        JexHealth.Hurt += Hurt;
        JexHealth.Eat += Eat;
        JexHealth.Die += Die;
    }

    private void UnregisterEvents()
    {
        BounceOffWall.OnAnnoyed -= Annoyed;
        JexHealth.Hurt -= Hurt;
        JexHealth.Eat -= Eat;
        JexHealth.Die -= Die;
    }

    private void Annoyed() => TryTrigger(HashAnnoyed);
    

    private void Hurt() => TryTrigger(HashHurt); 

    private void Eat()
    {
        if (!CanChangeState()) return;

        _animator.SetTrigger(_eatToggle ? HashEat : HashEat2);
        _eatToggle = !_eatToggle;
        _timeDelaySate = Time.time;
    }

    private void Die() => _animator.SetTrigger(HashDie);

    private void TryTrigger(int hash)
    {
        if(!CanChangeState()) return;
        _animator.SetTrigger(hash);
        _lastTimeState = Time.time;  
    }    

    private bool CanChangeState()
    {
       return Time.time - _lastTimeState >= _timeDelaySate;
    }    
}
