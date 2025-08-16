using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGunScreenUI : MonoBehaviour
{
    [Header("Border")]
    [SerializeField] List<GameObject> _borderSelected;

    [Header("Lock")]
    [SerializeField] List<GameObject> _imageLock;

    [Header("Unlock")]
    [SerializeField] List<GameObject> _imageUnlock;

    private List<GunSelected> _gunSelecteds;

    private void Start()   {
        
    }

    private void LoadData()
    {
        var count = _borderSelected.Count;
        if (count != _imageLock.Count || count != _imageUnlock.Count)
        {
            Debug.Log("Data GunSelected Not Valid");
            return;
        }


    }


}

[System.Serializable]
public class GunSelected
{
    public GameObject imageBorderSelected;
    public GameObject imageLock;
    public GameObject imageUnlock;
}