using UnityEditor;

namespace Framework.Editor
{
    [InitializeOnLoad]
    public static class EditorHandler
    {
        static EditorHandler()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            if (EditorApplication.isCompiling && EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
        }
    }
}