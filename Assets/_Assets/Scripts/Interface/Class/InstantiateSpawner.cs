
using UnityEngine;

public class InstantiateSpawner : IInstantiateSpawner
{
    public void Despawner(GameObject obj)
    {
       GameObject.Destroy(obj);
    }

    GameObject IInstantiateSpawner.Spawner(GameObject obj, Vector3 positionSpawner, Quaternion quaternion)
    {
        return GameObject.Instantiate(obj, positionSpawner, quaternion);
    }
}
