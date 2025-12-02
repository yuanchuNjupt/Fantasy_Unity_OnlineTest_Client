using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace UIFramework.Editor
{
    public class UISettingsWindow : OdinEditorWindow
    {
        private const string ViewPathKey = "UIFramework_ViewPath";
        private const string PresenterPathKey = "UIFramework_PresenterPath";
        
        private static string DefaultViewPath = Application.dataPath + "/Scripts/UIFramework/ViewPath/";
        private static string DefaultPresenterPath = Application.dataPath + "/Scripts/UIFramework/PresenterPath/";
        
        [LabelText("View生成路径")]
        [FolderPath]
        [OnValueChanged(nameof(OnViewPathChanged))]
        public string ViewPath = DefaultPresenterPath;
        
        [PropertySpace(20)]
        [LabelText("Presenter生成路径")]
        [FolderPath]
        [OnValueChanged(nameof(OnPresenterPathChanged))]
        public string PresenterPath = DefaultPresenterPath;

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadSettings();
        }

        private void LoadSettings()
        {
            ViewPath = GeneratorConfig.viewPath;
            PresenterPath = GeneratorConfig.presenterPath;
        }

        private void OnViewPathChanged()
        {
            GeneratorConfig.viewPath = ViewPath;
        }

        private void OnPresenterPathChanged()
        {
            GeneratorConfig.presenterPath = PresenterPath;
        }
    }
}