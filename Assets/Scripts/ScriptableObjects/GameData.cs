using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenderData", menuName = "Data/Game data")]
public class GameData : ScriptableObject
{
    public float timeLimit;
    public int energyBar;
    public int matchPerGame;
    public float attackerEnergyRegeneration;
    public int attackerEnergyCost;
    public float defenderEnergyRegeneration;
    public int defenderEnergyCost;
}
