namespace UIFramework.Editor
{
    /// <summary>
    /// 组件信息类
    /// </summary>
    public class ViewInfo
    {
        public int insID;
        public string fieldName;
        public string fieldType;

        public ViewInfo(int insID, string fieldName, string fieldType)
        {
            this.insID = insID;
            this.fieldName = fieldName;
            this.fieldType = fieldType;
        }
    }
}