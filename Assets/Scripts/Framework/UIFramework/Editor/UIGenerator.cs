using System;
using System.IO;
using UnityEditor;

namespace UIFramework.Editor
{
    
    /// <summary>
    /// 用于生成UIView和Presenter的脚本文件
    /// </summary>
    public class UIGenerator
    {
        

        /// <summary>
        /// UIView脚本代码
        /// </summary>
        private readonly string _viewCodeInfo;

        private readonly string _fileName;
        
        private readonly string _presenterCodeInfo;

        public UIGenerator(string viewCodeInfo ,string presenterCodeInfo ,string fileName)
        {
            _viewCodeInfo = viewCodeInfo;
            _fileName = fileName;
            _presenterCodeInfo = presenterCodeInfo;
        }

        public void GenerateViewFile()
        {
            
            if (!Directory.Exists(GeneratorConfig.viewPath))
            {
                Directory.CreateDirectory(GeneratorConfig.viewPath);
            }
            
            string filePath = GeneratorConfig.viewPath + _fileName + "View.cs";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using StreamWriter sw = File.CreateText(filePath);
            sw.Write(_viewCodeInfo);
            sw.Close();
            AssetDatabase.Refresh();
        }
        
        public void GeneratePresenterFile()
        {
            
            if (!Directory.Exists(GeneratorConfig.presenterPath))
            {
                Directory.CreateDirectory(GeneratorConfig.presenterPath);
            }
            
            string filePath = GeneratorConfig.presenterPath + _fileName + "Presenter.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            using StreamWriter sw = File.CreateText(filePath);
            sw.Write(_presenterCodeInfo);
            sw.Close();
            AssetDatabase.Refresh();
            
        }

        public bool PresenterFileExists()
        {
            string filePath = GeneratorConfig.presenterPath + _fileName + "Presenter.cs";
            return File.Exists(filePath);
        }
        
        
    }
}