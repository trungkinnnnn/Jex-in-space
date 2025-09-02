using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] ShockWaveData _data;

    private SpriteRenderer _sprite;
    private CircleCollider2D _collider;
    private Material _materialClone;
    private static int _para_WaveDistance = Shader.PropertyToID("_WaveDistanceFromCenter");
    
   
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _materialClone = Instantiate(_sprite.material);
        _sprite.material = _materialClone;

        gameObject.SetActive(false);
    }

    public void OnShockWave()
    {
        gameObject.SetActive(true);
        _collider.radius = _data.radiusColliderStart;
        _materialClone.SetFloat(_para_WaveDistance, _data.waveStartMat);

        DOTween.To(() => _collider.radius, x => _collider.radius = x, _data.radiusColliderEnd, _data.duration).SetEase(Ease.OutQuad);
        _materialClone.DOFloat(_data.waveEndMat, _para_WaveDistance, _data.duration).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector3 distance = (collision.transform.position - transform.position).normalized;
        rb.AddForce(distance * _data.forceEnter, ForceMode2D.Impulse);
    }


}
