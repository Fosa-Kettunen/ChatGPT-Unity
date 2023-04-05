namespace ChatGPTRequest.DataFormatter
{
    public struct StringOrArray
    {
        public StringOrArray(string str)
        {
            Values = str;
        }

        public StringOrArray(string[] arr)
        {
            Values = arr;
        }

        public object Values { get; }
    }
}