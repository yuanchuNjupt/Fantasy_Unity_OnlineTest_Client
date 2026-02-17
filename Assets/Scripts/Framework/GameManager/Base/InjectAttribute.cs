using System;

namespace Framework.GameManager.Base
{
    /// <summary>
    /// 依赖注入特性，用于标记需要自动注入的Manager成员
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
    }
}

