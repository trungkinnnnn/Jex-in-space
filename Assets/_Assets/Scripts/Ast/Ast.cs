
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

            Instantiate(effectAniDestroy, transform.position, Quaternion.identity);

            CreatLight2DExplosion();
            AstDestroy();
        }    
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
        var explosion = Instantiate(effectLight2DExplosion, transform.position, Quaternion.identity);
        EffectLightExplosion light = explosion.GetComponent<EffectLightExplosion>();
        light.Init(radiusExplosion);
    }    


    protected virtual void AstDestroy()
    {
        Destroy(gameObject);
    }

}
