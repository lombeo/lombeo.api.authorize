namespace Lombeo.Api.Authorize.DTO.CourseDTO
{
    public class RequestEnrollDTO : BaseRequest
    {
        public int CourseId { get; set; }
        public string InvoiceCode { get; set; }
        public IFormFile Image { get; set; }
    }
}
