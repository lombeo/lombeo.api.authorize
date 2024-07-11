namespace Lombeo.Api.Authorize.DTO.DoctorDTO
{
    public class SaveBookingDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string? Note { get; set; }
        public int DoctorId { get; set; }
        public int Shift { get; set; }
    }
}
