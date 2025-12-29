using UnityEngine;
using UnityEditor;
using NecromancersRising.Undead;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace NecromancersRising.Editor
{
    /// <summary>
    /// Fixes UndeadData assets that lost their moves list after script update
    /// </summary>
    public class UndeadDataMigration : EditorWindow
    {
        [MenuItem("Tools/Necromancer's Rising/Migrate All Undead Data")]
        public static void MigrateAllUndeadData()
        {
            // Find all UndeadData assets
            string[] guids = AssetDatabase.FindAssets("t:UndeadData");
            
            if (guids.Length == 0)
            {
                EditorUtility.DisplayDialog("No Assets Found", 
                    "No UndeadData assets found in the project!", 
                    "OK");
                return;
            }

            int fixedCount = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UndeadData data = AssetDatabase.LoadAssetAtPath<UndeadData>(path);
                
                if (data != null)
                {
                    // Force re-initialize the moves list
                    if (data.moves == null)
                    {
                        Debug.Log($"Fixing null moves list for: {data.name}");
                        data.moves = new List<Combat.MoveData>();
                    }
                    
                    // Mark dirty and save
                    EditorUtility.SetDirty(data);
                    fixedCount++;
                    
                    Debug.Log($"Migrated: {data.name} (moves count: {data.moves.Count})");
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("Migration Complete", 
                $"Fixed {fixedCount} UndeadData assets!\n\nPlease close and reopen your Zombie asset in the Inspector.", 
                "OK");
        }

        [MenuItem("Tools/Necromancer's Rising/Force Refresh Selected Undead")]
        public static void ForceRefreshSelected()
        {
            UndeadData selected = Selection.activeObject as UndeadData;
            
            if (selected == null)
            {
                EditorUtility.DisplayDialog("Error", 
                    "Please select an UndeadData asset in the Project window first!", 
                    "OK");
                return;
            }

            // Force Unity to completely reload the asset
            string path = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            
            EditorUtility.DisplayDialog("Refresh Complete", 
                $"Force refreshed: {selected.name}\n\nThe Inspector should now show the moves list correctly.", 
                "OK");
        }
    }
}
#endif