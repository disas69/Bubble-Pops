using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Signals.Editor
{
    [CustomPropertyDrawer(typeof(Signal))]
    public class SignalPropertyDrawer : PropertyDrawer
    {
        private int _selectedIndex = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var rect = new Rect(position.x, position.y, position.width, position.height);
            var signalsStorage = (SignalsStorage) AssetDatabase.LoadAssetAtPath(SignalsStorage.AssetPath + "SignalsStorage.asset", typeof(SignalsStorage));
            if (signalsStorage != null)
            {
                if (signalsStorage.Signals.Count > 0)
                {
                    var name = property.FindPropertyRelative("Name");

                    EditorGUI.BeginChangeCheck();
                    {
                        var index = Mathf.Max(0, signalsStorage.Signals.FindIndex(n => n == name.stringValue));
                        if (index != _selectedIndex)
                        {
                            _selectedIndex = index;
                            GUI.changed = true;
                        }

                        _selectedIndex = EditorGUI.Popup(rect, _selectedIndex, signalsStorage.Signals.ToArray());
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        name.stringValue = signalsStorage.Signals[_selectedIndex];
                    }
                }
                else
                {
                    if (GUI.Button(rect, "Add Signal"))
                    {
                        Selection.activeObject = signalsStorage;
                    }
                }
            }
            else
            {
                if (GUI.Button(rect, "Create Signals Storage"))
                {
                    if (!Directory.Exists(SignalsStorage.AssetPath))
                    {
                        Directory.CreateDirectory(SignalsStorage.AssetPath);
                    }

                    var localizationStorage = ScriptableObject.CreateInstance<SignalsStorage>();
                    AssetDatabase.CreateAsset(localizationStorage, SignalsStorage.AssetPath + "SignalsStorage.asset");
                }
            }

            EditorGUI.EndProperty();
        }
    }
}