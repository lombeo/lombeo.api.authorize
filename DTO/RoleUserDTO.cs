namespace Lombeo.Api.Authorize.DTO
{
    public class RoleUserDTO : BaseRoleUserDTO
    {

    }
    public class BaseRoleUserDTO
    {
        public int? UserId { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
