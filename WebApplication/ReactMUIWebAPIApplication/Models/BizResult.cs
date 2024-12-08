namespace ReactMUIWebAPIApplication.Models
{
    public class BizResult
    {
        public BizResult() { }

        public BizResult(int stsCode, string message, string data) {
            this.stsCode = stsCode;
            this.stsMessage = message;
            this.data = data;
        }

        public int stsCode { set; get; }
        public string stsMessage { set; get; }

        public string data { set; get; }

    }
}
