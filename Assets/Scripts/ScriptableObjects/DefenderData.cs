using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DefenderData", menuName = "Data/Defender data")]
public class DefenderData : ScriptableObject
{
    public float spawnTime;
    public float normalSpeed;
    public float returnSpeed;
    public float detectionField;
    public float reactivateTime;
    public Material detectionMaterial;
    public Material inactiveMaterial;
    public Material normalMaterial;
    public float inactivateDistance;
}
