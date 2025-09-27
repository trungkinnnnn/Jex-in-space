
using UnityEngine;

public interface ILeanPoolSpawner
{
    GameObject Spawner(GameObject obj, Vector3 positionSpawner, Quaternion quaternion);
    void Despawner(GameObject obj);
}
