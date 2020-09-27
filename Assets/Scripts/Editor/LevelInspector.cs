using TD.Data.Levels;
using UnityEditor;
using UnityEditor.Callbacks;

namespace TD.Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Double click this asset to open the Level Editor Window.", EditorStyles.centeredGreyMiniLabel);

            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_slotPrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_canConstructPrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_pathPrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_startingMoney"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_availableTowers"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_availableEnemies"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_numberOfWaves"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_wavesDuration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_timeBetweenWaves"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_initialEnemySpawnInterval"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_spawnRateTimeDecreasePerWave"));

            serializedObject.ApplyModifiedProperties();
        }

        [OnOpenAsset(0)]
        private static bool OnOpenEventTree(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj is Level)
            {
                LevelEditor.OpenWindow();
                return true;
            }

            return false;
        }
    }
}