using System.Collections;
using TMPro;
using UnityEngine;

public class BoxAmor : Ast
{
    public static System.Action<int> OnBoxBroken;
    private TextMeshProUGUI textProAmor;

    private string NAME_ANIMATION = "BoxAmor";
    private string NAME_ANI_BREAK = "isBreak";
    private float timeDestroy;

    public int maxAmor = 40;
    public int minAmor = 15;
    private int amorDrops;

    private int alphaEnd = 0;
    private int alphaStart = 1;

    private Animator _animator;

    public void Init(TextMeshProUGUI text)
    {
        textProAmor = text;
    }    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        amorDrops = Random.Range(minAmor, maxAmor);
        timeDestroy = GetLeghtClipByName(_animator, NAME_ANIMATION);
    }

    // Thực thi destroy
    protected override void AstDestroy()
    {
        Destroy(gameObject, timeDestroy);
        _animator.SetTrigger(NAME_ANI_BREAK);
        OnBoxBroken?.Invoke(amorDrops);

        SetTextProAmor(alphaStart);
        StartCoroutine(FadeAlpha(timeDestroy));
    }

    private void SetTextProAmor(int alpha)
    {
        textProAmor.transform.position = transform.position;
        textProAmor.text = "+" + amorDrops;
        SetAlpha(alpha);
    }    

    private IEnumerator FadeAlpha(float duration)
    {
        float timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(alphaStart, alphaEnd, timer/duration);
            SetAlpha(alpha);
            yield return null;
        }    
    }    

    private void SetAlpha(float a)
    {
        Color color = textProAmor.color;
        color.a = a;
        textProAmor.color = color;
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
