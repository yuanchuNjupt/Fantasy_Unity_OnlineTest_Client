using System.IO;
using System.Text;
using UnityEditor;

namespace Framework.GameManagerFramework.Editor
{
    public class GameManagerGenerator
    {

        private string _generateInfo;
        
        public enum GenerateType
        {
            LogicManager,
            DataManager,
            MessageManager,
        }
        
        public static void GenerateWorld(string worldName)
        {
            if (!Directory.Exists(GameFrameworkDataManager.WorldGeneratePath))
            {
                Directory.CreateDirectory(GameFrameworkDataManager.WorldGeneratePath);
            }
            
            string filePath = GameFrameworkDataManager.WorldGeneratePath + worldName +".cs";
            
            var sw = new StreamWriter(filePath);
            
            sw.Write(GenerateWorldCode(worldName));
            sw.Close();
            AssetDatabase.Refresh();
            
            
        }

        public static void GenerateManager(string managerName, string worldName, GenerateType generateType)
        {
            
            string filePath;
            switch (generateType)
            {
                case GenerateType.LogicManager:
                    if (!Directory.Exists(GameFrameworkDataManager.LogicManagerGeneratePath))
                    {
                        Directory.CreateDirectory(GameFrameworkDataManager.LogicManagerGeneratePath);
                    }
                    filePath = GameFrameworkDataManager.LogicManagerGeneratePath + managerName +"LogicManager.cs";
                    break;
                case GenerateType.DataManager:
                    if (!Directory.Exists(GameFrameworkDataManager.DataManagerGeneratePath))
                    {
                        Directory.CreateDirectory(GameFrameworkDataManager.DataManagerGeneratePath);
                    }
                    filePath = GameFrameworkDataManager.DataManagerGeneratePath + managerName +"DataManager.cs";
                    break;
                case GenerateType.MessageManager:
                    if (!Directory.Exists(GameFrameworkDataManager.MessageManagerGeneratePath))
                    {
                        Directory.CreateDirectory(GameFrameworkDataManager.MessageManagerGeneratePath);
                    }
                    filePath = GameFrameworkDataManager.MessageManagerGeneratePath + managerName + "MessageManager.cs";
                    break;
                default:
                    filePath = "";
                    break;
            }
            
            var sw = new StreamWriter(filePath);
            
            sw.Write(GenerateManagerCode(managerName, worldName, generateType));
            sw.Close();
            AssetDatabase.Refresh();
        }

        private static string GenerateWorldCode(string worldName)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("//此World由GameManager框架自动生成"); 
            sb.AppendLine("//注:所有的World必须由GameManager框架进行生成，否则无法加入到管理系统中!");
            sb.AppendLine("//作者:原初z");
            sb.AppendLine();
            
            sb.AppendLine("public class " + worldName + " : World");
            sb.AppendLine("{");
            sb.AppendLine("\t");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GenerateManagerCode(string managerName, string worldName, GenerateType generateType)
        {
            var sb = new StringBuilder();
            switch (generateType)
            {
                case GenerateType.DataManager:
                    sb.AppendLine("//此DataManager由GameManager框架自动生成"); 
                    sb.AppendLine("//注:所有的DataManager必须由GameManager框架进行生成，否则无法加入到管理系统中!");
                    
                    break;
                case GenerateType.LogicManager:
                    sb.AppendLine("//此LogicManager由GameManager框架自动生成");
                    sb.AppendLine("//注:所有的LogicManager必须由GameManager框架进行生成，否则无法加入到管理系统中!");
                    break;
                case GenerateType.MessageManager:
                    sb.AppendLine("//此MessageManager由GameManager框架自动生成");
                    sb.AppendLine("//注:所有的MessageManager必须由GameManager框架进行生成，否则无法加入到管理系统中!");
                    break;
            }
            sb.AppendLine("//作者:原初z");

            sb.AppendLine();
            sb.AppendLine($"[WorldSource(typeof({worldName}))");
            switch (generateType)
            {
                case GenerateType.DataManager:
                    sb.AppendLine($"public class {managerName}DataManager : IDataManager");
                    break;
                case GenerateType.LogicManager:
                    sb.AppendLine($"public class {managerName}LogicManager : ILogicManager");
                    break;
                case GenerateType.MessageManager:
                    sb.AppendLine($"public class {managerName}MessageManager : IMessageManager");
                    break;
            }

            sb.AppendLine("{");

            sb.AppendLine("\tpublic void OnCreate()");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine();
            sb.AppendLine("\tpublic void OnDestroy()");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            return sb.ToString();



        }
        
        
    }
}