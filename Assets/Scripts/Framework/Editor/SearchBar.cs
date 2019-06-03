using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class SearchBar
    {
        private const string ControllName = "SearchFilterInput";

        private string _searchString = string.Empty;
        private string _stringFilter = string.Empty;

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(_stringFilter); }
        }

        public void Draw()
        {
            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            {
                GUI.SetNextControlName(ControllName);

                _searchString = EditorGUILayout.TextField(_searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
                _stringFilter = _searchString.ToLowerInvariant();

                if (!string.IsNullOrEmpty(_stringFilter))
                {
                    if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                    {
                        _searchString = string.Empty;
                        GUI.FocusControl(null);
                    }
                }
                else
                {
                    GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButtonEmpty"));
                }
            }
            GUILayout.EndHorizontal();
        }

        public bool IsMatchingTheFilter(string value)
        {
            return !string.IsNullOrEmpty(value) && value.ToLowerInvariant().Contains(_stringFilter);
        }
    }
}