using Game.Data;
using Game.Data.Settings;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class GameConfigurationWindow : EditorWindow
    {
        private SerializedObject _serializedObject;
        private Vector2 _settingsContentPosition;
        private Vector2 _colorsContentPosition;
        private Vector2 _levelsContentPosition;
        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(GameConfiguration.Instance);

            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            _labelStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };
            
            GameData.Load();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawWindowLayout();

            _serializedObject.ApplyModifiedProperties();
            _serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(GameConfiguration.Instance);
            }
        }

        private void DrawWindowLayout()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(285f), GUILayout.ExpandHeight(true));
                {
                    _settingsContentPosition = EditorGUILayout.BeginScrollView(_settingsContentPosition);
                    {
                        DrawDataSettings();
                        DrawGridSettings();
                        DrawRowSettings();
                        DrawPopSettings();
                        DrawColorSettings();
                        DrawEffectsSettings();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawLevels();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDataSettings()
        {
            EditorGUILayout.LabelField("Data", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                GameData.Data.Level = EditorGUILayout.IntField("Level", GameData.Data.Level);
                GameData.Data.Score = EditorGUILayout.IntField("Score", GameData.Data.Score);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Apply"))
                    {
                        GameData.Save();
                    }

                    if (GUILayout.Button("Reset"))
                    {
                        FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/" + GameData.FileName);
                        GameData.Load();
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                if (GUILayout.Button("Play"))
                {
                    GameEditor.PlayGame();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawGridSettings()
        {
            EditorGUILayout.LabelField("Grid", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var gridSettings = _serializedObject.FindProperty("GridSettings");
                EditorGUILayout.PropertyField(gridSettings.FindPropertyRelative("StartRowCount"));
                EditorGUILayout.PropertyField(gridSettings.FindPropertyRelative("RowMinCount"));
                EditorGUILayout.PropertyField(gridSettings.FindPropertyRelative("RowMaxCount"));
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawRowSettings()
        {
            EditorGUILayout.LabelField("Row", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var rowSettings = _serializedObject.FindProperty("RowSettings");
                EditorGUILayout.PropertyField(rowSettings.FindPropertyRelative("RowShift"));
                EditorGUILayout.PropertyField(rowSettings.FindPropertyRelative("RowHeight"));
                EditorGUILayout.PropertyField(rowSettings.FindPropertyRelative("MoveTime"));
                EditorGUILayout.PropertyField(rowSettings.FindPropertyRelative("MoveStep"));
                EditorGUILayout.PropertyField(rowSettings.FindPropertyRelative("MoveCurve"));
            }
            EditorGUILayout.EndVertical();
        }
        
        private void DrawPopSettings()
        {
            EditorGUILayout.LabelField("Pop", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var popSettings = _serializedObject.FindProperty("PopSettings");
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MoveTime"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MoveCurve"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MergeTime"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MergeCurve"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MinUpForce"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MaxUpForce"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("ReactEffect"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("MergeEffect"));
                EditorGUILayout.PropertyField(popSettings.FindPropertyRelative("BlowUpEffect"));
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawColorSettings()
        {
            EditorGUILayout.LabelField("Colors", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("Add color"))
                {
                    RecordObject("Color Settings Change");
                    GameConfiguration.Instance.Colors.Add(new ColorSettings());
                }

                var colors = _serializedObject.FindProperty("Colors");
                var count = colors.arraySize;
                for (var i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = colors.GetArrayElementAtIndex(i);

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("Value"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("Color"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("Chance"));
                        }
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Color Settings Change");
                            GameConfiguration.Instance.Colors.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEffectsSettings()
        {
            EditorGUILayout.LabelField("Effects", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("Add effect"))
                {
                    RecordObject("Effect Settings Change");
                    GameConfiguration.Instance.Effects.Add(new EffectsSettings());
                }

                var effects = _serializedObject.FindProperty("Effects");
                var count = effects.arraySize;
                for (var i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = effects.GetArrayElementAtIndex(i);

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("Name"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("Prefab"));
                        }
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Effects Settings Change");
                            GameConfiguration.Instance.Effects.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawLevels()
        {
            EditorGUILayout.LabelField("Levels", _headerStyle);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("Add level"))
                {
                    RecordObject("Level Settings Change");

                    var levelSettings = new LevelSettings();
                    if (GameConfiguration.Instance.Levels.Count > 0)
                    {
                        var prevLevelSettings = GameConfiguration.Instance.Levels[GameConfiguration.Instance.Levels.Count - 1];
                        levelSettings.Copy(prevLevelSettings);
                    }
                    GameConfiguration.Instance.Levels.Add(levelSettings);
                }

                _levelsContentPosition = EditorGUILayout.BeginScrollView(_levelsContentPosition);
                {
                    var levels = _serializedObject.FindProperty("Levels");
                    var count = levels.arraySize;
                    for (var i = 0; i < count; i++)
                    {
                        var element = levels.GetArrayElementAtIndex(i);
                        var level = element.FindPropertyRelative("Level");
                        var score = element.FindPropertyRelative("Score");
                        
                        element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, $"Level {level.intValue} : {score.intValue}", true);
                        if (element.isExpanded)
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.PropertyField(level);
                                    EditorGUILayout.PropertyField(score);
                                }
                                EditorGUILayout.EndVertical();

                                if (GUILayout.Button("X", GUILayout.Width(20)))
                                {
                                    RecordObject("Level Settings Change");
                                    GameConfiguration.Instance.Levels.RemoveAt(i);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        private void RecordObject(string changeDescription = "Game Configuration change")
        {
            Undo.RecordObject(_serializedObject.targetObject, changeDescription);
        }
    }
}