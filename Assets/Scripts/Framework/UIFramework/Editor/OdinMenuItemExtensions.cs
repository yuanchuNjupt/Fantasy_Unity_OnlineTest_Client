using UIFramework.View;
using UnityEditor;
using UnityEngine;

namespace UIFramework.Editor
{
    public class OdinMenuItemExtensions
    {
        
        [MenuItem("UI/MVP生成器")]
        public static void ShowViewGeneratorWindow()
        {
            var window = EditorWindow.GetWindow<ViewGeneratorWindow>();
            window.Show();
        }
        
        [MenuItem("CONTEXT/BaseView/绑定View组件")]
        public static void BindViewField(MenuCommand command)
        {
            ViewBinder binder = new ViewBinder(command.context);
            binder.Bind();
        }

        [MenuItem("GameObject/配置MVP")]
        public static void ChooseGameObjectOpenWindow()
        {
            GameObject go = Selection.gameObjects[0];
            
            var window = EditorWindow.GetWindow<ViewGeneratorWindow>();
            window.Show();
            window.generateObject = go;
            window.Preview();
        }

        [MenuItem("GameObject/自动切换UITag #V")]
        public static void AutoSwitchTag()
        {
            GameObject[] gos = Selection.gameObjects;
            TagSwitcher.SwitchTag(gos);
            
        }
    }
}