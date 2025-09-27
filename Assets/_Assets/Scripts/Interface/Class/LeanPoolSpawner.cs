
using UnityEngine;
using Lean.Pool;

public class LeanPoolSpawner : ILeanPoolSpawner
{
    public void Despawner(GameObject obj)
    {
       LeanPool.Despawn(obj);   
    }

    GameObject ILeanPoolSpawner.Spawner(GameObject obj, Vector3 positionSpawner, Quaternion quaternion)
    {
       return LeanPool.Spawn(obj, positionSpawner, quaternion);   
    }
}
