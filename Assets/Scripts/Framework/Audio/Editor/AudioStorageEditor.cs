using Framework.Audio.Configuration;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Audio.Editor
{
    [CustomEditor(typeof(AudioStorage))]
    public class AudioStorageEditor : CustomEditorBase<AudioStorage>
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
                EditorGUILayout.LabelField("Audio Clips", HeaderStyle);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    _searchBar.Draw();

                    if (GUILayout.Button("Add clip"))
                    {
                        RecordObject("Music Storage Change");
                        Target.AudioClips.Add(new AudioClipConfig());
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            var clipsList = serializedObject.FindProperty("AudioClips");
            var count = clipsList.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    int searchResultsCount = 0;
                    for (int i = 0; i < count; i++)
                    {
                        var element = clipsList.GetArrayElementAtIndex(i);
                        var key = element.FindPropertyRelative("Key");
                        var audioClip = element.FindPropertyRelative("AudioClip");
                        var capacity = element.FindPropertyRelative("Capacity");

                        if (!_searchBar.IsEmpty && !_searchBar.IsMatchingTheFilter(key.stringValue))
                        {
                            continue;
                        }

                        searchResultsCount++;
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.PropertyField(key);
                                EditorGUILayout.PropertyField(audioClip);
                                EditorGUILayout.PropertyField(capacity);
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Music Storage Change");
                                Target.AudioClips.RemoveAt(i);
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