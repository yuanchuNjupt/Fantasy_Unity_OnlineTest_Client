using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework.Editor
{
    /// <summary>
    /// 自动切换标签
    /// </summary>
    public static class TagSwitcher
    {
        
        public static void SwitchTag(IEnumerable<GameObject> gameObjects)
        {
            foreach (GameObject go in gameObjects)
            {
                if (go.name.Contains("[") && go.name.Contains("]"))
                {
                    RemoveTags(go);
                }
                else
                {
                    AddTags(go);
                }
                
            }
            
        }

        private static void AddTags(GameObject go)
        {
            string name = "Transform";
            Component[] components = go.GetComponents<Component>();
            foreach (Component component in components)
            {
                switch (component)
                {
                    case Text:
                        name = nameof(Text);
                        break;
                    case Image:
                        name = nameof(Image);
                        break;
                    case Button:
                        name = nameof(Button);
                        break;
                    case InputField:
                        name = nameof(InputField);
                        break;
                    case Dropdown:
                        name = nameof(Dropdown);
                        break;
                    case ScrollRect:
                        name = nameof(ScrollRect);
                        break;
                    case Slider:
                        name = nameof(Slider);
                        break;
                    case Toggle:
                        name = nameof(Toggle);
                        break;
                    case RawImage:
                        name = nameof(RawImage);
                        break;
                    case TextMeshProUGUI:
                        name = nameof(TextMeshProUGUI);
                        break;
                }
            }
            go.name = $"[{name}]{go.name}";
            
        }

        private static void RemoveTags(GameObject go)
        {
            int index = go.name.IndexOf("]") + 1;
            string name = go.name.Substring(index , go.name.Length - index);
            go.name = name;
        }
        
    }
}