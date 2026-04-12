namespace Battle.FrameCommand
{
    public enum FrameCommandType
    {
        None = 0,
        Prediction = 1, // 客户端预测指令
        Authoritative = 2, // 权威指令
    }
}