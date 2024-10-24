namespace Lombeo.Api.Authorize.DTO.DoctorDTO
{
    public class SaveDoctorDTO
    {
        public int Id { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public string BriefInfo { get; set; }
        public string Room { get; set; }
        public string Location { get; set; }
        public List<int> Shift { get; set; }
        public double Price { get; set; }
        public List<DoctorInfo> Info { get; set; }
    }
}
