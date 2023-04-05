
using ChatGPTRequest.DataFormatter;
namespace ChatGPTRequest
{
    public struct ModelSettings
    {
        public string model;
        public float temperature;
        public float top_p;
        public bool stream;
        //public StringOrArray stop;
        public int max_tokens;
        public float presence_penalty;
        public float frequency_penalty;
        //public string logit_bias;
    }
}