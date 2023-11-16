namespace Do_An_Chuyen_Nganh.Service.Payment
{
    public class MomoSettings
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string EndPoint { get; set; }
        public string RedirectUrl { get; set; }
        public string IpnUrl { get; set; }
        public int resultCode { get; set; }

    }
}