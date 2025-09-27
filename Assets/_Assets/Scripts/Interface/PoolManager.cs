using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private ILeanPoolSpawner _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
        _pool = SpawnerFactory.GetSpawner();
    }

    public GameObject Spawner(GameObject obj, Vector3 position, Quaternion quaternion)
    {
        return _pool.Spawner(obj, position, quaternion);
    }

    public void Despawner(GameObject obj)
    {
        _pool.Despawner(obj);
    }

}
