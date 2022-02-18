namespace Api.Model
{
    public class SignatureDTO
    {
        public string Reason { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public int SignatureLength { get; set; }
        public string Passphrase { get; set; }
        public int[] Rect { get; set; }
        public string PdfBuffer { get; set; }
        public string P12Buffer { get; set; }
    }
}