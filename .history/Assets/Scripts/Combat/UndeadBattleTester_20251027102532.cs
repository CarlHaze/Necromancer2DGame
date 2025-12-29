using UnityEngine;
using NecromancersRising.Combat;
using NecromancersRising.Undead;

/// <summary>
/// Test script that simulates battles between the 3 starter undead
/// Runs multiple simulations and reports win rates
/// Uses Monte-Carlo simulation rules with move system
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
    public int battlesPerMatchup = 1000;

    private void Start()
    {
        // Validate that all data is assigned
        if (skeletonData == null || zombieData == null || ghoulData == null)
        {
            Debug.LogError("Please assign all 3 undead data assets in the Inspector!");
            return;
        }

        Debug.Log("=== STARTING UNDEAD BATTLE SIMULATIONS ===");
        Debug.Log($"Running {battlesPerMatchup} battles per matchup");
        Debug.Log("NOW USING MOVE SYSTEM WITH TYPE EFFECTIVENESS!\n");

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
            string winner = SimulateBattle(fighter1, fighter2, fighter1Data, fighter2Data);

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
        Debug.Log($"{fighter2Data.undeadName} wins: {fighter2Wins}/{battlesPerMatchup} ({fighter2WinRate:F1}%)\n");
    }

    /// <summary>
    /// Simulates a single battle between two undead using their moves
    /// Uses Monte-Carlo simulation rules with move system
    /// </summary>
    private string SimulateBattle(UndeadStats fighter1, UndeadStats fighter2, UndeadData fighter1Data, UndeadData fighter2Data)
    {
        // Battle loop - continue until one or both die
        while (fighter1.IsAlive && fighter2.IsAlive)
        {
            // Determine turn order based on MODIFIED speed (includes buffs/debuffs)
            UndeadStats firstAttacker;
            UndeadStats secondAttacker;
            UndeadData firstAttackerData;
            UndeadData secondAttackerData;

            int fighter1Speed = fighter1.GetModifiedSpeed();
            int fighter2Speed = fighter2.GetModifiedSpeed();

            if (fighter1Speed > fighter2Speed)
            {
                firstAttacker = fighter1;
                secondAttacker = fighter2;
                firstAttackerData = fighter1Data;
                secondAttackerData = fighter2Data;
            }
            else if (fighter2Speed > fighter1Speed)
            {
                firstAttacker = fighter2;
                secondAttacker = fighter1;
                firstAttackerData = fighter2Data;
                secondAttackerData = fighter1Data;
            }
            else
            {
                // Speed tie - random 50/50
                if (Random.value < 0.5f)
                {
                    firstAttacker = fighter1;
                    secondAttacker = fighter2;
                    firstAttackerData = fighter1Data;
                    secondAttackerData = fighter2Data;
                }
                else
                {
                    firstAttacker = fighter2;
                    secondAttacker = fighter1;
                    firstAttackerData = fighter2Data;
                    secondAttackerData = fighter1Data;
                }
            }

            // First attacker's turn - choose random move
            ExecuteMove(firstAttacker, secondAttacker, firstAttackerData);

            // Check if second attacker died
            if (!secondAttacker.IsAlive)
            {
                return firstAttacker.undeadName;
            }

            // Second attacker's turn - choose random move
            ExecuteMove(secondAttacker, firstAttacker, secondAttackerData);

            // Check for simultaneous KO (both die in same round)
            if (!firstAttacker.IsAlive && !secondAttacker.IsAlive)
            {
                // Simultaneous KO - random winner (50/50)
                return Random.value < 0.5f ? fighter1.undeadName : fighter2.undeadName;
            }

            // Check if first attacker died
            if (!firstAttacker.IsAlive)
            {
                return secondAttacker.undeadName;
            }
        }

        // Fallback (should never reach here)
        return fighter1.IsAlive ? fighter1.undeadName : fighter2.undeadName;
    }

    /// <summary>
    /// Executes a random move from the attacker's known moves
    /// </summary>
    private void ExecuteMove(UndeadStats attacker, UndeadStats defender, UndeadData attackerData)
    {
        // Safety check
        if (attackerData.knownMoves == null || attackerData.knownMoves.Count == 0)
        {
            Debug.LogError($"{attackerData.undeadName} has no moves!");
            return;
        }

        // Choose random move
        MoveData move = attackerData.knownMoves[Random.Range(0, attackerData.knownMoves.Count)];

        // Check accuracy
        int hitRoll = Random.Range(1, 101);
        if (hitRoll > move.accuracy)
        {
            // Miss - no effect
            return;
        }

        // Apply move based on effect type
        if (move.IsDamaging)
        {
            // Calculate and apply damage
            int damage = CalculateMoveDamage(attacker, defender, move);
            defender.TakeDamage(damage);
        }
        else if (move.IsStatMove)
        {
            // Apply stat change
            if (move.target == MoveTarget.Self)
            {
                attacker.ModifyStatStage(move.statModified, move.statChangeAmount);
            }
            else // MoveTarget.Enemy
            {
                defender.ModifyStatStage(move.statModified, move.statChangeAmount);
            }
        }
    }

    /// <summary>
    /// Calculates damage from a move with type effectiveness
    /// Formula: (Attacker.ModifiedAttack * MovePower/50 - Defender.ModifiedDefense/2) * TypeEffectiveness
    /// </summary>
    private int CalculateMoveDamage(UndeadStats attacker, UndeadStats defender, MoveData move)
    {
        // Get modified stats (includes buffs/debuffs)
        float attackerAttack = attacker.GetModifiedAttack();
        float defenderDefense = defender.GetModifiedDefense();

        // Base damage calculation with move power
        float baseDamage = (attackerAttack * move.basePower / 50f) - (defenderDefense / 2f);

        // Apply type effectiveness
        float typeMultiplier = TypeChart.Instance.GetEffectiveness(move.moveType, defender.type);

        // Calculate final damage
        float finalDamage = baseDamage * typeMultiplier;

        // Ensure minimum 1 damage on hit
        return Mathf.Max(1, Mathf.RoundToInt(finalDamage));
    }
}