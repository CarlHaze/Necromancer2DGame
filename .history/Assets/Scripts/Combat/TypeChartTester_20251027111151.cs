// using UnityEngine;
// using NecromancersRising.Combat;

// /// <summary>
// /// Test script to verify TypeChart effectiveness calculations
// /// Attach to a GameObject in the scene to run tests
// /// </summary>
// public class TypeChartTester : MonoBehaviour
// {
//     // Run tests when the game starts
//     private void Start()
//     {
//         Debug.Log("=== STARTING TYPE CHART TESTS ===");
//         RunAllTests();
//         Debug.Log("=== TYPE CHART TESTS COMPLETE ===");
//     }

//     private void RunAllTests()
//     {
//         // Test 1: Super Effective matchups
//         TestSuperEffective();

//         // Test 2: Not Very Effective matchups
//         TestNotEffective();

//         // Test 3: Neutral matchups
//         TestNeutral();
//     }

//     /// <summary>
//     /// Tests super effective (2x damage) matchups
//     /// </summary>
//     private void TestSuperEffective()
//     {
//         Debug.Log("--- Testing Super Effective Matchups ---");

//         // Infernal beats Frost
//         float result1 = TypeChart.Instance.GetEffectiveness(UndeadType.Infernal, UndeadType.Frost);
//         LogTest("Infernal vs Frost", result1, 2.0f);

//         // Frost beats Plague
//         float result2 = TypeChart.Instance.GetEffectiveness(UndeadType.Frost, UndeadType.Plague);
//         LogTest("Frost vs Plague", result2, 2.0f);

//         // Soul beats Dark
//         float result3 = TypeChart.Instance.GetEffectiveness(UndeadType.Soul, UndeadType.Dark);
//         LogTest("Soul vs Dark", result3, 2.0f);

//         // Soul beats Spirit
//         float result4 = TypeChart.Instance.GetEffectiveness(UndeadType.Soul, UndeadType.Spirit);
//         LogTest("Soul vs Spirit", result4, 2.0f);
//     }

//     /// <summary>
//     /// Tests not very effective (0.5x damage) matchups
//     /// </summary>
//     private void TestNotEffective()
//     {
//         Debug.Log("--- Testing Not Very Effective Matchups ---");

//         // Spirit attacks Bone (Bone weak to Spirit)
//         float result1 = TypeChart.Instance.GetEffectiveness(UndeadType.Spirit, UndeadType.Bone);
//         LogTest("Spirit vs Bone", result1, 0.5f);

//         // Infernal attacks Plague (Plague weak to Fire)
//         float result2 = TypeChart.Instance.GetEffectiveness(UndeadType.Infernal, UndeadType.Plague);
//         LogTest("Infernal vs Plague", result2, 0.5f);

//         // Bone attacks Feral (Feral weak to Bone)
//         float result3 = TypeChart.Instance.GetEffectiveness(UndeadType.Bone, UndeadType.Feral);
//         LogTest("Bone vs Feral", result3, 0.5f);

//         // Dark attacks Spirit (Spirit weak to Dark)
//         float result4 = TypeChart.Instance.GetEffectiveness(UndeadType.Dark, UndeadType.Spirit);
//         LogTest("Dark vs Spirit", result4, 0.5f);
//     }

//     /// <summary>
//     /// Tests neutral (1.0x damage) matchups
//     /// </summary>
//     private void TestNeutral()
//     {
//         Debug.Log("--- Testing Neutral Matchups ---");

//         // Bone vs Plague (no special interaction)
//         float result1 = TypeChart.Instance.GetEffectiveness(UndeadType.Bone, UndeadType.Plague);
//         LogTest("Bone vs Plague", result1, 1.0f);

//         // Hex vs Bone (no special interaction)
//         float result2 = TypeChart.Instance.GetEffectiveness(UndeadType.Hex, UndeadType.Bone);
//         LogTest("Hex vs Bone", result2, 1.0f);

//         // Blight vs Spirit (no special interaction)
//         float result3 = TypeChart.Instance.GetEffectiveness(UndeadType.Blight, UndeadType.Spirit);
//         LogTest("Blight vs Spirit", result3, 1.0f);
//     }

//     /// <summary>
//     /// Helper method to log test results
//     /// </summary>
//     private void LogTest(string matchup, float actual, float expected)
//     {
//         if (Mathf.Approximately(actual, expected))
//         {
//             Debug.Log($"PASS: {matchup} = {actual}x (Expected: {expected}x)");
//         }
//         else
//         {
//             Debug.LogError($"FAIL: {matchup} = {actual}x (Expected: {expected}x)");
//         }
//     }
// }