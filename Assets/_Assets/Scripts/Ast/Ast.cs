using System;
using UnityEngine;

public class Ast : MonoBehaviour
{
    private Action onDestroyed;

    public int hp = 2;

    private void Start()
    {
        BulletBase.OnhitAst += OnBulletHit;
    }
    private void OnDestroy()
    {
        onDestroyed?.Invoke();
        BulletBase.OnhitAst -= OnBulletHit;
    }

    public void Init(Action onDestroyed)
    {
        this.onDestroyed = onDestroyed;
       
    }

    private void OnBulletHit(Ast ast)
    {
        if(ast != this) return;
        hp -= 1;
        
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }    


   

}
