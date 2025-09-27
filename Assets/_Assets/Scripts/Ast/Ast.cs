
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidType { AstNormal, AstExplosion, AstNon}

public class Ast : MonoBehaviour
{
    // WaveSpawner
    private System.Action OnDestroyAst;
    // PlayerInventory, CheckingAstTutorial
    public static System.Action<int, AsteroidType> AddScoreOnDie;

    public AsteroidType type = AsteroidType.AstNormal;

    [Header("OnDestroy")]
    [SerializeField] GameObject _objBroken;
    [SerializeField] GameObject _effectLight2DExplosion;
    [SerializeField] GameObject _effectAniDestroy;
    public int hp = 1;
    public float radiusExplosion = 1f;

    [Header("Coin")]
    [SerializeField] CoinDropData _coinData;
    [SerializeField] GameObject _coinPrefab;
    private int _quanityCoin;

    [Header("CameraShake")]
    [SerializeField] CameraShakeData _shakeData;

    [Header("Score")]
    [SerializeField] protected int _score = 1;

    [Header("Audio")]    
    [SerializeField] AudioData _audioDataHit;
    [SerializeField] AudioData _audioImpact;
    [SerializeField] AudioData _audioBreak;
    [SerializeField] AudioData _audioCrack;
    public float volumeNormal = 1f;
    public float volumeExplosion = 1.4f;

    public List<AudioClip> GetAudioHitBulletList() => _audioDataHit == null ? null : _audioDataHit.clipList;

    public void InitOnDestroy(System.Action onDestroy)
    {
        OnDestroyAst = onDestroy;
    }

    public void InitAddScore(int score)
    {
        _score = score;
    }    

    public void PlayAudioImpact(float perSpeed)
    {
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioImpact.clipList, perSpeed);
    }

    public void PlayAudioCrack()
    {
        AudioSFX.Instance.PlayAudioOneShortChangeVolume(_audioCrack.clipList, volumeNormal + 2f);
    }

    private void Start()
    {
        if(_coinData != null) _quanityCoin = Random.Range(_coinData.quanityMin, _coinData.quanityMax);
    }

    private void OnDestroy()
    {
        OnDestroyAst?.Invoke();
    }

    public void TakeDameBullet(int dmage)
    {
        hp -= dmage;

        if (hp == 0)
        {
            AstDestroy();
        }
        else
        {
            OnBroken();
        }    
    }

    protected virtual void AstDestroy()
    {
        CreateAniDestroy();
        CreatLight2DExplosion();
        CreateCoins();
        AddScoreOnDie?.Invoke(_score, type);
        Destroy(gameObject);
    }

    private void OnBroken()
    {
        if (_objBroken != null)
        {
            PlayAudioCrack();
            _objBroken.SetActive(true);
        }
    }    

    private void CreatLight2DExplosion()
    {
        if (_effectLight2DExplosion == null) return;
        var explosion = Instantiate(_effectLight2DExplosion, transform.position, Quaternion.identity);
        EffectLightExplosion light = explosion.GetComponent<EffectLightExplosion>();
        light.InitRadius(radiusExplosion);
        if (_shakeData == null) return;
        light.InitCameraShake(_shakeData.range, _shakeData.intensity);
    }    

    private void CreateAniDestroy()
    {
        if (_effectAniDestroy == null) return;
        var obj = Instantiate(_effectAniDestroy, transform.position, Quaternion.identity);
        if (_audioBreak == null || _audioBreak.clipList.Count == 0) return;
        EffectController effect = obj.GetComponent<EffectController>();
        effect.InitAudioVolumDownBackground(_audioBreak.clipList, volumeExplosion);
    }    

    private void CreateCoins()
    {
        if(_coinData == null) return;   
        for (int i = 0; i < _quanityCoin; i++)
        {
            PoolManager.Instance.Spawner(_coinPrefab, transform.position, Quaternion.identity);
        }
    }    
   

}
