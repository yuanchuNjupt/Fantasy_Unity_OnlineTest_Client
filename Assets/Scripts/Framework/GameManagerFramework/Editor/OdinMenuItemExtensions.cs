using UnityEditor;

namespace Framework.GameManagerFramework.Editor
{
    public class OdinMenuItemExtensions
    {
        [MenuItem("GameManager/打开GameManager框架窗口")]
        public static void ShowGameManagerFrameworkWindow()
        {
            var window = EditorWindow.GetWindow<GameManagerWindow>();
            window.Show();



        }
    }
}