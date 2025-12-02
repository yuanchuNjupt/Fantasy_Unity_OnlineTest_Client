using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UIFramework.View;
using UnityEditor;
using UnityEngine;

namespace UIFramework.Editor
{
    public class ViewGeneratorWindow : OdinEditorWindow
    {
        
        private const string Title = "View代码生成";

        private const string Horizontal = Title + "/Split";

        private const string LeftVertical = Horizontal + "/LeftArea";

        private const string RightVertical = Horizontal + "/RightArea";

        private const string LeftBoxGroup = LeftVertical + "/GenerateSetting";

        private const string RightBoxGroup = RightVertical + "/Preview";
        
        
        [BoxGroup(LeftBoxGroup)]
        [LabelText("生成对象")]
        public GameObject generateObject;

        [TitleGroup(Title, Alignment = TitleAlignments.Centered)]
        [HorizontalGroup(Horizontal, width: 400)]
        [VerticalGroup(LeftVertical)]
        [BoxGroup(LeftBoxGroup, LabelText = "生成设置")]
        [LabelText("命名空间")]
        [ReadOnly]
        public string classNamespace = "UIFramework.ViewPath";
        
        [HorizontalGroup(Horizontal)]
        [VerticalGroup(RightVertical)]
        [BoxGroup(RightBoxGroup, LabelText = "生成预览")]
        [TextArea(40, 40)]
        [HideLabel]
        public string previewInfo;

        [BoxGroup(LeftBoxGroup)]
        [Button(ButtonSizes.Large, Name = "预览代码"), GUIColor("blue")]
        public void Preview()
        {
            if (generateObject == null)
            {
                Debug.LogError("生成对象为空");
                return;
            }

            UITemplate template = new UITemplate(classNamespace);
            previewInfo = template.BuildViewTemplate(generateObject.transform);
            
        }

        [BoxGroup(LeftBoxGroup)]
        [Button(ButtonSizes.Large, Name = "仅生成View代码"), GUIColor("green")]
        public void GenerateView()
        {
            if (generateObject == null)
            {
                Debug.LogError("生成对象为空");
                return;
            }
            
            UITemplate template = new UITemplate(classNamespace);
            previewInfo = template.BuildViewTemplate(generateObject.transform);
            UIGenerator generator = new UIGenerator(previewInfo ,template.BuildPresenterTemplate(generateObject.transform) ,generateObject.name);
            generator.GenerateViewFile();
            
        }


        [BoxGroup(LeftBoxGroup)]
        [InfoBox("警告：此操作将生成并覆盖现有文件，请谨慎操作！", InfoMessageType.Error)]
        [PropertyTooltip("将会生成View和Presenter文件")]
        [Button(ButtonSizes.Large, Name = "生成全部代码"), GUIColor("yellow")]
        public void GenerateAll()
        {
            if (generateObject == null)
            {
                Debug.LogError("生成对象为空");
                return;
            }
            UITemplate template = new UITemplate(classNamespace);
            previewInfo = template.BuildViewTemplate(generateObject.transform);
            UIGenerator generator = new UIGenerator(previewInfo ,template.BuildPresenterTemplate(generateObject.transform) ,generateObject.name);

            if (generator.PresenterFileExists())
            {
                //如果存在Presenter文件，进行二次确认
                bool confirmed = EditorUtility.DisplayDialog(
                    "确认生成",
                    $"确定要为 [{generateObject.name}] 生成全部代码吗？\n\n此操作将覆盖现有的Presenter 文件！",
                    "确定生成",
                    "取消"
                );

                if (!confirmed)
                {
                    Debug.Log("已取消生成操作");
                    return;
                }
            }
            
            generator.GenerateViewFile();
            generator.GeneratePresenterFile();
            
            Debug.Log($"已成功生成 [{generateObject.name}] 的全部代码！");
        }
        
        
    }
}
