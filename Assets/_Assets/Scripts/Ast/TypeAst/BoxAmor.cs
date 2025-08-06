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

    private Animator _animator;

    public void Init(TextMeshProUGUI text)
    {
        textProAmor = text;
    }    

    private void Start()
    {
        _animator = GetComponent<Animator>();

        SetTextStart();

        amorDrops = Random.Range(minAmor, maxAmor);
        timeDestroy = GetLeghtClipByName(_animator, NAME_ANIMATION);
    }

    private void SetTextStart()
    {
        if (textProAmor == null) return;
        textProAmor.gameObject.SetActive(false);
        SetAlpha(1f);
    }

    // Thực thi destroy
    protected override void AstDestroy()
    {
        _animator.SetTrigger(NAME_ANI_BREAK);
        OnBoxBroken?.Invoke(amorDrops);

        SetTextProAmor();
        StartCoroutine(FadeAlpha(timeDestroy));

        Destroy(gameObject, timeDestroy);
    }

  

    private void SetTextProAmor()
    {
        textProAmor.gameObject.SetActive(true);
        textProAmor.transform.position = transform.position;
        textProAmor.text = "+" + amorDrops;
        SetAlpha(1f);
    }    

    private IEnumerator FadeAlpha(float duration)
    {
        float timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer/duration);
            SetAlpha(alpha);
            yield return null;
        }    
        SetAlpha(0f);
        textProAmor.gameObject.SetActive(false);
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
