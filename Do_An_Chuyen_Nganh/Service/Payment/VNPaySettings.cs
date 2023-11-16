namespace Do_An_Chuyen_Nganh.Service.Payment
{
    public class VNPaySettings
    {
        public string HashSecret { get; set; }
        public string TmnCode { get; set; }
        public string Url { get; set; }
        public string ReturnUrl { get; set; }
    }
}
