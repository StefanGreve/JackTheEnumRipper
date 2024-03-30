namespace JackTheEnumRipper.Models
{
    public record AppSettings
    {
        public required string Encoding { get; set; }

        public string? Comment { get; set; }

        public string? Indentation { get; set; }
    }
}
