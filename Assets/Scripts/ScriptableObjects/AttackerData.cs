using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AttackerData", menuName ="Data/Attacker data")]
public class AttackerData : ScriptableObject
{
    public float spawnTime;
    public float CatchBallDistance;
    public float ScorePointDistance;
    public float normalSpeed;
    public float carryingSpeed;
    public float ballSpeed;
    public float reactivateTime;
    public Material inactiveMaterial;
    public Material normalMaterial;

}
