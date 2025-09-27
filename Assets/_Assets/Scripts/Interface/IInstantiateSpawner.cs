
using UnityEngine;
public interface IInstantiateSpawner
{
    GameObject Spawner(GameObject obj, Vector3 positionSpawner, Quaternion quaternion);
    void Despawner(GameObject obj);
}
