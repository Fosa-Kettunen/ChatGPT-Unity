using ChatGPTRequest.DataFormatter;

namespace ChatGPTRequest.apiData {
    public struct ApiDataPackage
    {
        public ApiDataPackage(Message message, float executionTime = 0f,string finnish_reason = "")
        {
            this.Message = message;
            this.ExecutionTime = executionTime;
            this.finnish_reason = finnish_reason;
        }
        public Message Message;
        public float ExecutionTime;
        public string finnish_reason;
    }
}