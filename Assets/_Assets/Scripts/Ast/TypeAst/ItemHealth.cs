using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : Ast
{
    // Xử lý logic phía player
    public void HandleDestroyHealth()
    {
        AstDestroy();
    }

    protected override void AstDestroy()
    {
        base.AstDestroy();
    }
}
