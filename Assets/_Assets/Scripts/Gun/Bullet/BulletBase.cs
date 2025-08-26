
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    //Data
    [SerializeField] protected List<GameObject> _effectHits;
    public string COLORHEXA = "FFC015";
    [SerializeField] protected GameObject _lightEffect;

    //Para
    private Vector2 _direction;
    private float _speed;
    private const float _timeLife = 3f;
    private const int _damage = 1;
    

    [Header("PARA EXPLOSION LIGHT")]
    public float radiusEffectLight = 0.4f;
    public float forceStay = 0.8f;
    public float forceEnter = 0.5f;
   
    protected string NAME_COMPARETAG_PHYSIC = "Ast";
    protected string NAME_COMPARETAG_PHYSIC_ITEM = "Health";


    //Audio
    private List<AudioClip> _audioClipList;

    //Component
    private Rigidbody2D _rb;

    public void Init(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _direction * _speed;
        Destroy(gameObject, _timeLife);
    }

    // Hiệu ứng va chạm
    protected virtual void CreateEffectHit()
    {
        GameObject prefabEffect = RandomEffect();
        var objEffect = Instantiate(prefabEffect, transform.position, Quaternion.identity);
        var effect = objEffect.GetComponent<EffectController>();
        effect?.ApplyHexColor(COLORHEXA);

        if (_audioClipList != null && _audioClipList.Count > 0)
            effect?.InitAudioOneShortNormal(_audioClipList);

        CreateEffectLight();
    }    


    // Hiệu ứng ánh sáng
    protected virtual void CreateEffectLight()
    {
        if (_lightEffect == null) return;
        GameObject effect = Instantiate(_lightEffect, transform.position, Quaternion.identity);
        var effectLight = effect.GetComponent<EffectLightExplosion>();
        effectLight?.InitRadius(radiusEffectLight);
        effectLight?.InitForce(forceEnter, forceStay);
    }    

    protected GameObject RandomEffect()
    {
        if(_effectHits.Count == 1) return _effectHits[0];
        return _effectHits[Random.Range(0, _effectHits.Count)];
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(NAME_COMPARETAG_PHYSIC) || other.CompareTag(NAME_COMPARETAG_PHYSIC_ITEM))
        {
            AstHitBullet(other);
            HandleHitAst(other);
        }
    }

    protected abstract void HandleHitAst(Collider2D other);

    private void AstHitBullet(Collider2D other)
    {
        var obj = other.GetComponent<Ast>();
        _audioClipList = obj.GetAudioHitBulletList();
        obj?.TakeDameBullet(_damage);
    }

}
