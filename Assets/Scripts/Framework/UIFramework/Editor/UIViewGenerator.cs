using System;
using System.IO;
using UnityEditor;

namespace UIFramework.Editor
{
    
    /// <summary>
    /// 用于生成UIView脚本文件
    /// </summary>
    public class UIViewGenerator
    {
        

        /// <summary>
        /// UIView脚本代码
        /// </summary>
        private readonly string _viewCodeInfo;

        private readonly string _fileName;

        public UIViewGenerator(string viewCodeInfo , string fileName)
        {
            _viewCodeInfo = viewCodeInfo;
            _fileName = fileName;
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
            
            
        }
    }
}