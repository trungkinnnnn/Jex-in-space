using System.Collections;
using TMPro;
using UnityEngine;

public class BoxAmor : Ast
{
    // Gun controller
    public static System.Action<int> OnBoxBroken;
    private TextMeshProUGUI _textProAmor;

    private string NAME_ANIMATION = "BoxAmor";
    private string NAME_ANI_BREAK = "isBreak";
    private float _timeDestroy;
    private int _amorDrops;
    private int _alphaEnd = 0;
    private int _alphaStart = 1;

    public int maxAmor = 40;
    public int minAmor = 15;

    private Animator _animator;

    public void Init(TextMeshProUGUI text)
    {
        _textProAmor = text;
    }    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _amorDrops = Random.Range(minAmor, maxAmor);
        _timeDestroy = GetLeghtClipByName(_animator, NAME_ANIMATION);
    }

    // Thực thi destroy
    protected override void AstDestroy()
    {
        Destroy(gameObject, _timeDestroy);
        _animator.SetTrigger(NAME_ANI_BREAK);
        OnBoxBroken?.Invoke(_amorDrops);
        AddScoreOnDie?.Invoke(_score);
        SetTextProAmor(_alphaStart);
        StartCoroutine(FadeAlpha(_timeDestroy));
    }

    private void SetTextProAmor(int alpha)
    {
        _textProAmor.transform.position = transform.position;
        _textProAmor.text = "+" + _amorDrops;
        SetAlpha(alpha);
    }    

    private IEnumerator FadeAlpha(float duration)
    {
        float timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(_alphaStart, _alphaEnd, timer/duration);
            SetAlpha(alpha);
            yield return null;
        }    
    }    

    private void SetAlpha(float a)
    {
        Color color = _textProAmor.color;
        color.a = a;
        _textProAmor.color = color;
    }


    private float GetLeghtClipByName(Animator animator, string nameAni)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == nameAni)
            {
                return clip.length;
            }
        }
        Debug.Log("Not find Animation");
        return 0;
    }

}
