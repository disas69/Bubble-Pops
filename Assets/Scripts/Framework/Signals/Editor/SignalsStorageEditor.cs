using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Signals.Editor
{
    [CustomEditor(typeof(SignalsStorage))]
    public class SignalsStorageEditor : CustomEditorBase<SignalsStorage>
    {
        private SearchBar _searchBar;

        protected override void OnEnable()
        {
            base.OnEnable();
            _searchBar = new SearchBar();
        }

        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Signals Storage", HeaderStyle);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    _searchBar.Draw();

                    if (GUILayout.Button("Add Signal"))
                    {
                        RecordObject("Signals Storage Change");
                        Target.Signals.Add(string.Empty);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            var signals = serializedObject.FindProperty("Signals");
            var count = signals.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    int searchResultsCount = 0;
                    for (int i = 0; i < count; i++)
                    {
                        var element = signals.GetArrayElementAtIndex(i);

                        if (!_searchBar.IsEmpty && !_searchBar.IsMatchingTheFilter(element.stringValue))
                        {
                            continue;
                        }

                        searchResultsCount++;
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.PropertyField(element, new GUIContent(string.Format("Signal {0}", i + 1)));
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Signals Storage Change");
                                Target.Signals.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    if (searchResultsCount == 0)
                    {
                        EditorGUILayout.LabelField("No matches found...", LabelStyle);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();
        }
    }
}