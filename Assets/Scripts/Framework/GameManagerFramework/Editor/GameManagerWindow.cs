using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Framework.GameManagerFramework.Editor
{
    public class GameManagerWindow : OdinEditorWindow
    {
        //生成World
        //生成LogicManager
        //生成DataManager
        //生成MessageManager
        //管理所有World
        //管理所有Manager
        //生成执行顺序脚本
        //管理执行顺序
        
        //分为创建 / 管理 两个模块
        
        [TabGroup("GameManager" , "创建模块" , TextColor = "lightmagenta")]
        public CreateModelConfig CreateModelConfig = new CreateModelConfig();
        
        [TabGroup("GameManager" , "管理模块" , TextColor = "lightblue")]
        public ControlModelConfig ControlModelConfig = new ControlModelConfig();
        
            
    }
}