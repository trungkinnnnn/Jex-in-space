
using UnityEngine;

public class Ast : MonoBehaviour
{

    private System.Action OnDestroyAst;

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

    public void Init(System.Action onDestroy)
    {
        OnDestroyAst = onDestroy;
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

        OnBroken();

        if (hp == 0)
        {
            AstDestroy();
        }    
    }

    protected virtual void AstDestroy()
    {
        CreateAniDestroy();
        CreatLight2DExplosion();
        CreateCoins();
        Destroy(gameObject);
    }

    private void OnBroken()
    {
        if (_objBroken != null)
        {
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
        Instantiate(_effectAniDestroy, transform.position, Quaternion.identity);
    }    

    private void CreateCoins()
    {
        if(_coinData == null) return;   
        for (int i = 0; i < _quanityCoin; i++)
        {
            Instantiate(_coinPrefab, transform.position, Quaternion.identity);
        }
    }    
   

}
