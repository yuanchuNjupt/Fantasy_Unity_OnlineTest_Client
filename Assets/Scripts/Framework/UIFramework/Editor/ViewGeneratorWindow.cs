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

            UIViewTemplate template = new UIViewTemplate(classNamespace);
            previewInfo = template.BuildViewTemplate(generateObject.transform);
            
        }

        [BoxGroup(LeftBoxGroup)]
        [Button(ButtonSizes.Large, Name = "生成代码"), GUIColor("green")]
        public void Generate()
        {
            if (generateObject == null)
            {
                Debug.LogError("生成对象为空");
                return;
            }


            UIViewTemplate template = new UIViewTemplate(classNamespace);
            previewInfo = template.BuildViewTemplate(generateObject.transform);
            

            
            UIViewGenerator generator = new UIViewGenerator(previewInfo , generateObject.name);
            generator.GenerateViewFile();
            
            
            //存储信息
            // string dataListJson = JsonConvert.SerializeObject(template.objViewInfoList);
            // EditorPrefs.SetString("GeneratorViewDataList" , dataListJson);
            //
            // EditorPrefs.SetString("GeneratorViewClassName" , generateObject.name + "View");
            
            
            
        }
        
        
        

        // 自动挂载，但是非常吃性能，直接作废
  
        /*
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void AddViewComponent()
        {
            string className = EditorPrefs.GetString("GeneratorViewClassName");
            if (string.IsNullOrEmpty(className))
            {
                return;
            }
            
            //1.反射获取脚本并挂载
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            //找到Csharp程序集
            var csharpAssembly = assemblies.First(a => a.GetName().Name == "Assembly-CSharp");
            //获取类所在成员及路径
            string relClassName = "UIFramework.ViewPath." + className;

            Type type = csharpAssembly.GetType(relClassName);
            if (type == null)
            {
                Debug.LogError("未找到类型：" + relClassName + "自动挂载失败");
                EditorPrefs.DeleteKey("GeneratorViewDataList");
                EditorPrefs.DeleteKey("GeneratorViewClassName");
                return;
            }
               
            //获取要查找的物体
            string windowObjName = className.Replace("View", "");
            GameObject viewObj = GameObject.Find(windowObjName);
            if (viewObj == null)
            {
                Debug.LogError("未找到物体：" + windowObjName + "自动挂载失败");
                EditorPrefs.DeleteKey("GeneratorViewDataList");
                EditorPrefs.DeleteKey("GeneratorViewClassName");
                return;
            }
            
            Component c = viewObj.GetComponent(type);
            if (c == null)
            {
                c = viewObj.AddComponent(type);
            }
            
            

            //2.通过反射遍历数据列表找到对应字段，赋值
            string dataListJson = EditorPrefs.GetString("GeneratorViewDataList");
            List<ViewInfo> dataList = JsonConvert.DeserializeObject<List<ViewInfo>>(dataListJson);
            
            IEnumerable<FieldInfo> fieldInfos = type.GetFields();

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                foreach (ViewInfo viewInfo in dataList)
                {
                    if (fieldInfo.Name == viewInfo.fieldName)
                    {
                        //根据ID获取GameObject
                        GameObject uiObject = EditorUtility.InstanceIDToObject(viewInfo.insID) as GameObject;
                        if (uiObject == null)
                        {
                            Debug.LogError("未找到GameObject：" + viewInfo.insID + "自动挂载失败");
                            continue;
                        }

                        if (string.Equals(viewInfo.fieldType, "GameObject"))
                        {
                            fieldInfo.SetValue(c, uiObject);
                        }
                        else
                        {
                            fieldInfo.SetValue(c, uiObject.GetComponent(viewInfo.fieldType));
                        }
                        break;
                    }
                }
            }

            EditorPrefs.DeleteKey("GeneratorViewDataList");
            EditorPrefs.DeleteKey("GeneratorViewClassName");
        }
        
        */
        
    }
}
