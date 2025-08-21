
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreenUI : MonoBehaviour
{
    public static Action Die;

    public void ActionDie()
    {
        Die?.Invoke();
    }
}
