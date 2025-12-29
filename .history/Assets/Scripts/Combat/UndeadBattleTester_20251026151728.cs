using UnityEngine;
using NecromancersRising.Combat;
using NecromancersRising.Undead;

/// <summary>
/// Test script that simulates battles between the 3 starter undead
/// Runs multiple simulations and reports win rates
/// </summary>
public class UndeadBattleTester : MonoBehaviour
{
    [Header("Undead Data Assets")]
    [Tooltip("Drag the Skeleton asset here")]
    public UndeadData skeletonData;

    [Tooltip("Drag the Zombie asset here")]
    public UndeadData zombieData;

    [Tooltip("Drag the Ghoul asset here")]
    public UndeadData ghoulData;

    [Header("Simulation Settings")]
    [Tooltip("Number of battles to simulate for each matchup")]
    public int battlesPerMatchup = 100;

    private void Start()
    {
        // Validate that all data is assigned
        if (skeletonData == null || zombieData == null || ghoulData == null)
        {
            Debug.LogError("Please assign all 3 undead data assets in the Inspector!");
            return;
        }

        Debug.Log("=== STARTING UNDEAD BATTLE SIMULATIONS ===");
        Debug.Log($"Running {battlesPerMatchup} battles per matchup\n");

        RunAllMatchups();

        Debug.Log("=== BATTLE SIMULATIONS COMPLETE ===");
    }

    /// <summary>
    /// Runs all possible 1v1 matchups between the 3 starters
    /// </summary>
    private void RunAllMatchups()
    {
        // Skeleton vs Zombie
        RunMatchup(skeletonData, zombieData);

        // Skeleton vs Ghoul
        RunMatchup(skeletonData, ghoulData);

        // Zombie vs Ghoul
        RunMatchup(zombieData, ghoulData);
    }

    /// <summary>
    /// Simulates multiple battles between two undead types
    /// </summary>
    private void RunMatchup(UndeadData fighter1Data, UndeadData fighter2Data)
    {
        int fighter1Wins = 0;
        int fighter2Wins = 0;

        for (int i = 0; i < battlesPerMatchup; i++)
        {
            // Create fresh instances for each battle
            UndeadStats fighter1 = fighter1Data.CreateInstance();
            UndeadStats fighter2 = fighter2Data.CreateInstance();

            // Simulate battle
            string winner = SimulateBattle(fighter1, fighter2);

            if (winner == fighter1.undeadName)
            {
                fighter1Wins++;
            }
            else
            {
                fighter2Wins++;
            }
        }

        // Calculate win rates
        float fighter1WinRate = (float)fighter1Wins / battlesPerMatchup * 100f;
        float fighter2WinRate = (float)fighter2Wins / battlesPerMatchup * 100f;

        // Report results
        Debug.Log($"--- {fighter1Data.undeadName} vs {fighter2Data.undeadName} ---");
        Debug.Log($"{fighter1Data.undeadName} wins: {fighter1Wins}/{battlesPerMatchup} ({fighter1WinRate:F1}%)");
        Debug.Log($"{fighter2Data.undeadName} wins: {fighter2Wins}/{battlesPerMatchup} ({fighter2WinRate:F1}%)");
        Debug.Log($"Type Effectiveness: {fighter1Data.undeadName} deals {TypeChart.Instance.GetEffectiveness(fighter1Data.type, fighter2Data.type)}x damage");
        Debug.Log($"Type Effectiveness: {fighter2Data.undeadName} deals {TypeChart.Instance.GetEffectiveness(fighter2Data.type, fighter1Data.type)}x damage\n");
    }

    /// <summary>
    /// Simulates a single battle between two undead
    /// Returns the name of the winner
    /// </summary>
    private string SimulateBattle(UndeadStats fighter1, UndeadStats fighter2)
    {
        // Determine turn order based on speed
        UndeadStats firstAttacker;
        UndeadStats secondAttacker;

        if (fighter1.speed >= fighter2.speed)
        {
            firstAttacker = fighter1;
            secondAttacker = fighter2;
        }
        else
        {
            firstAttacker = fighter2;
            secondAttacker = fighter1;
        }

        // Battle loop - continue until one dies
        while (fighter1.IsAlive && fighter2.IsAlive)
        {
            // First attacker's turn
            if (firstAttacker.IsAlive)
            {
                int damage = CalculateDamage(firstAttacker, secondAttacker);
                secondAttacker.TakeDamage(damage);

                if (!secondAttacker.IsAlive)
                {
                    return firstAttacker.undeadName;
                }
            }

            // Second attacker's turn
            if (secondAttacker.IsAlive)
            {
                int damage = CalculateDamage(secondAttacker, firstAttacker);
                firstAttacker.TakeDamage(damage);

                if (!firstAttacker.IsAlive)
                {
                    return secondAttacker.undeadName;
                }
            }
        }

        // Fallback (should never reach here)
        return fighter1.IsAlive ? fighter1.undeadName : fighter2.undeadName;
    }

    /// <summary>
    /// Calculates damage from attacker to defender
    /// Formula: (Attack - Defense/2) * Type Effectiveness
    /// Basic attacks are typeless - type effectiveness will come from moves later
    /// </summary>
    private int CalculateDamage(UndeadStats attacker, UndeadStats defender)
    {
        // Check if attack hits based on accuracy
        int hitRoll = Random.Range(1, 101); // 1-100
        if (hitRoll > attacker.accuracy)
        {
            // Miss!
            return 0;
        }

        // Base damage calculation
        float baseDamage = attacker.attack - (defender.defense / 2f);

        // TYPE EFFECTIVENESS REMOVED FOR BASIC ATTACKS
        // This will be added back when we implement the move system
        // For now, all basic attacks deal neutral (1x) damage

        // Calculate final damage
        float finalDamage = baseDamage * 1.0f; // Always neutral

        // Ensure minimum 1 damage on hit
        return Mathf.Max(1, Mathf.RoundToInt(finalDamage));
    }
}