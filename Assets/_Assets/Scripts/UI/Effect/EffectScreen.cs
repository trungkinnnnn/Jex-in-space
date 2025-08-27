using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EffectScreen : MonoBehaviour
{
    public static System.Action<float, float> onTakeDamage;

    [SerializeField] Material _mat;
    [SerializeField] Image _imageEffect;
    [SerializeField] Sprite _spriteEffectHealth;
    [SerializeField] Sprite _spriteEffectTakeDamage;

    private const string _para_IntensityX = "_IntensityX";
    private const string _para_IntensityY = "_IntensityY";
    private int _currentHp = 9;

    public float[] intensityX = { -0.1f, 0.1f };
    public float[] intensityY = { -0.1f, 0.1f };
    public float[] duration = { 0.2f, 0.5f };
    public float durationHold = 0.05f;

    public float range = 3f;
    public float intensity = 0.05f;

    private void Start()
    {
        SetAlpha(0f);
        _mat.SetFloat(_para_IntensityX, 0f);
        _mat.SetFloat(_para_IntensityY, 0f);
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents()
    {
        PlayerHealth.OnActionHp += HandleActionEffect;
    }

    private void UnRegisterEvents()
    {
        PlayerHealth.OnActionHp -= HandleActionEffect;
    }

    private void HandleActionEffect(int hp)
    {
        if (hp < _currentHp)
            EffectTakeDamage();
        else
            EffectHealth();

        _currentHp = hp;
    }

    private void EffectHealth()
    {
        _imageEffect.sprite = _spriteEffectHealth;

        Sequence seq = DOTween.Sequence();

        seq.Append(_imageEffect.DOFade(1f, duration[0]));
        seq.AppendInterval(durationHold);
        seq.Append(_imageEffect.DOFade(0f, duration[1]));

        seq.Play();
    }    

    private void EffectTakeDamage()
    {
        float startX = 0f;
        float startY = 0f;

        float targetX = Random.Range(intensityX[0], intensityX[1]);
        float targetY = Random.Range(intensityY[0], intensityY[1]);

        _imageEffect.sprite = _spriteEffectTakeDamage;

        Sequence seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => _mat.GetFloat(_para_IntensityX), x => _mat.SetFloat(_para_IntensityX, x), targetX, duration[0]));
        seq.Join(DOTween.To(() => _mat.GetFloat(_para_IntensityY), y => _mat.SetFloat(_para_IntensityY, y), targetY, duration[0]));
        seq.Join(_imageEffect.DOFade(1f, duration[0]));

        seq.AppendInterval(durationHold);

        seq.Append(DOTween.To(() => _mat.GetFloat(_para_IntensityX), x => _mat.SetFloat(_para_IntensityX, x), startX, duration[1]));
        seq.Join(DOTween.To(() => _mat.GetFloat(_para_IntensityY), y => _mat.SetFloat(_para_IntensityY, y), startY, duration[1]));
        seq.Join(_imageEffect.DOFade(0f, duration[1]));

        seq.Play();

        onTakeDamage?.Invoke(range, intensity);
    }    

    private void SetAlpha(float alpha)
    {
        Color color = _imageEffect.color;
        color.a = alpha;
        _imageEffect.color = color;
    }    

}
