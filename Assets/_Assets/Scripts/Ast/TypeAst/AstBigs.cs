using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstBigs : Ast
{
    public List<GameObject> CreatAst;

    protected override void AstDestroy()
    {
        CreatObj();
        base.AstDestroy();
    }

    private void CreatObj()
    {
        foreach (var obj in CreatAst)
        {
            if (obj != null)
            {
                obj.transform.SetParent(null);
                obj.SetActive(true);
            }    
        }    
    }

}
