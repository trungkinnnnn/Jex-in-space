using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstTracker : MonoBehaviour
{
    private Action onDestroyed;

    public void Init(Action onDestroyed)
    {
        this.onDestroyed = onDestroyed;
    }

    public void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
