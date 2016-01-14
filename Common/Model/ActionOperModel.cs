namespace Common.Model
{
    /// <summary>
    /// 异步委托调用时,返回的操作结果
    /// </summary>
    public class ActionOperModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Mark = false;
        /// <summary>
        /// 操作返回的提示信息
        /// </summary>
        public string MarkMessage = "";
    }
}