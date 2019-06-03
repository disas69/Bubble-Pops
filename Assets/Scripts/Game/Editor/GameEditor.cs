using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Editor
{
    public static class GameEditor
    {
        [MenuItem("Game/Play", priority = 1)]
        public static void PlayGame()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
            EditorApplication.isPlaying = true;
        }
        
        [MenuItem("Game/Configuration", priority = 2)]
        public static void OpenWindow()
        {
            var window = EditorWindow.GetWindow<GameConfigurationWindow>("Game Configuration");
            window.minSize = new Vector2(650f, 450f);
            window.Show();
        }
    }
}