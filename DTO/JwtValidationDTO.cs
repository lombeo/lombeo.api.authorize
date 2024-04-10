namespace Lombeo.Api.Authorize.DTO
{
    public class JwtValidationDTO
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
    }
}
