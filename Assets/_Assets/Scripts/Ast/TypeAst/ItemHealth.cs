using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : Ast
{
    public void HandleDestroyHealth()
    {
        AstDestroy();
    }

    protected override void AstDestroy()
    {
        base.AstDestroy();
    }
}
