
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Ast : MonoBehaviour
{

    private Action onDestroy;

    [SerializeField] GameObject objBroken;
    [SerializeField] GameObject effectLight2DExplosion;
    [SerializeField] GameObject effectAniDestroy;

    public int hp = 1;
    public float radiusExplosion = 1f;

    public void Init(Action onDestroy)
    {
        this.onDestroy = onDestroy;
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }

    public void TakeDameBullet(int dmage)
    {
        hp -= dmage;

        OnBroken();

        if (hp <= 0)
        {
            AstDestroy();
        }    
    }

    protected virtual void AstDestroy()
    {
        CreateAniDestroy();
        CreatLight2DExplosion();
        Destroy(gameObject);
    }

    private void OnBroken()
    {
        if (objBroken != null)
        {
            objBroken.SetActive(true);
        }
    }    

    private void CreatLight2DExplosion()
    {
        if (effectLight2DExplosion == null) return;
        var explosion = Instantiate(effectLight2DExplosion, transform.position, Quaternion.identity);
        EffectLightExplosion light = explosion.GetComponent<EffectLightExplosion>();
        light.Init(radiusExplosion);
    }    

    private void CreateAniDestroy()
    {
        if (effectAniDestroy == null) return;
        Instantiate(effectAniDestroy, transform.position, Quaternion.identity);
    }    

   

}
