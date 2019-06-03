using Framework.Editor;
using Framework.UI.Configuration;
using UnityEditor;
using UnityEngine;

namespace Framework.UI.Editor
{
    [CustomEditor(typeof(ScreensMapping))]
    public class ScreensMappingEditor : CustomEditorBase<ScreensMapping>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Pages", HeaderStyle);
                if (GUILayout.Button("Add page"))
                {
                    RecordObject("Screens Mapping Change");
                    Target.ScreenSettings.Add(null);
                }

                var screenSettings = serializedObject.FindProperty("ScreenSettings");
                var count = screenSettings.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = screenSettings.GetArrayElementAtIndex(i);
                        var screen = element.FindPropertyRelative("Screen");
                        var isCached = element.FindPropertyRelative("IsCached");

                        var screenReference = Target.ScreenSettings[i].Screen;
                        var elementName = screenReference != null ? screenReference.GetType().Name : "Screen";

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.PropertyField(screen, new GUIContent(elementName));
                            EditorGUILayout.PropertyField(isCached);
                        }
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Screens Mapping Change");
                            Target.ScreenSettings.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Popups", HeaderStyle);
                if (GUILayout.Button("Add popup"))
                {
                    RecordObject("Screens Mapping Change");
                    Target.PopupSettings.Add(null);
                }

                var popupSettings = serializedObject.FindProperty("PopupSettings");
                if (popupSettings.arraySize > 0)
                {
                    var count = popupSettings.arraySize;
                    for (int i = 0; i < count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var element = popupSettings.GetArrayElementAtIndex(i);
                            var poppup = element.FindPropertyRelative("Popup");

                            var popupReference = Target.PopupSettings[i].Popup;
                            var elementName = popupReference != null ? popupReference.GetType().Name : "Popup";

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.PropertyField(poppup, new GUIContent(elementName));
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Screens Mapping Change");
                                Target.PopupSettings.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}