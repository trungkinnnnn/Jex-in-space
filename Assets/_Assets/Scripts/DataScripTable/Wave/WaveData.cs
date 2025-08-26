using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Wave/WaveData"))]
public class WaveData : ScriptableObject
{
    public List<GameObject> bigAts;
    public List<GameObject> mediumAts;
    public List<GameObject> smallAts;
    public List<GameObject> goldAts;
    public List<GameObject> explosionAst;
    public List<GameObject> itemHealth;
    public List<GameObject> amorBox;
}
