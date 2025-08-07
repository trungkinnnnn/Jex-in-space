using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationListener : MonoBehaviour
{

    private const string NAME_ANI_TRIGGER_ANNOYED = "isAnnoyed";
    private const string NAME_ANI_TRIGGER_HURT = "isHurt";
    private const string NAME_ANI_TRIGGER_EAT = "isEat";
    private const string NAME_ANI_TRIGGER_EAT2 = "isEat2";
    private const string NAME_ANI_TRIGGER_DIE = "isDie";

    private float timeDelaySate = 0.3f;
    private float lastTimeState = 0f;   

    private bool checkAni = false;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        JexHeatlh.Hurt += Hurt;
        JexHeatlh.Eat += Eat;
        JexHeatlh.Die += Die;
    }

    private void UnregisterEvents()
    {
        BounceOffWall.OnAnnoyed -= Annoyed;
        JexHeatlh.Hurt -= Hurt;
        JexHeatlh.Eat -= Eat;
        JexHeatlh.Die -= Die;
    }

    private void Annoyed()
    {
        if(checkTimeState()) animator.SetTrigger(NAME_ANI_TRIGGER_ANNOYED);
    }    

    private void Hurt()
    {
        if (checkTimeState()) animator.SetTrigger(NAME_ANI_TRIGGER_HURT);
    }  

    private void Eat()
    {
        if(checkTimeState())
        {
            if (checkAni)
            {
                animator.SetTrigger(NAME_ANI_TRIGGER_EAT);
                checkAni = false;
            }
            else
            {
                animator.SetTrigger(NAME_ANI_TRIGGER_EAT2);
                checkAni = true;
            }
        }    
    }

    private void Die()
    {
        animator.SetTrigger(NAME_ANI_TRIGGER_DIE);
    }

    private bool checkTimeState()
    {
        if(Time.time - lastTimeState >= timeDelaySate)
        {
            lastTimeState = Time.time;
            return true;
        }    
        return false;
    }    
}
