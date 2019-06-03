using Framework.Editor;
using Game.Spawn;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : CustomEditorBase<Spawner>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Spawn Settings", HeaderStyle);
                
                var activateOnAwake = serializedObject.FindProperty("_activateOnAwake");
                EditorGUILayout.PropertyField(activateOnAwake);
                
                var spawnOnDestroySettings = serializedObject.FindProperty("_settings");
                var objectPrefab = spawnOnDestroySettings.FindPropertyRelative("ObjectPrefab");
                var poolCapacity = spawnOnDestroySettings.FindPropertyRelative("PoolCapacity");

                EditorGUILayout.PropertyField(objectPrefab);
                EditorGUILayout.PropertyField(poolCapacity);
            }
            EditorGUILayout.EndVertical();
        }
    }
}