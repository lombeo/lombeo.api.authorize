using Lombeo.Api.Authorize.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombeo.Api.Authorize.DTO
{
    [Keyless]
    public class UserDTO : BaseUserDTO
    {

    }
    public class BaseUserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public DateTime? BirthYear { get; set; }

        public string PhoneNumber { get; set; }

        public string CityName { get; set; }

        public string GraduatedSchool { get; set; }

        public string AvatarUrl { get; set; }
        [NotMapped]
        public UserExpLevelDTO UserExpLevel { get; set; }
        public string? Facebook { get; set; }

        public string? Twitter { get; set; }

        public string? LinkedIn { get; set; }

        public string? CompanyName { get; set; }

        public string? Achievement { get; set; }
    }
    public class RelationUserDTO : UserDTO
    {
        public RelationShipStatusEnum Status { get; set; }
        public int OwnerId { get; set; }
    }
    public class UserExpLevelDTO
    {
        public int UserLevelId { get; set; }
        public string Name { get; set; }
        public string CssClassName { get; set; }
        public string IconUrl { get; set; }
        public int LevelNo { get; set; }
        public decimal? AbsoluteExperiencePoint { get; set; }
        public decimal? ProgressToNextLevel { get; set; }
        public decimal? CurrentUserExperiencePoint { get; set; }
        public decimal? RelativeExperiencePoint { get; set; }
        public decimal? NextLevelExp { get; set; }
        public object NextLevelIconUrl { get; set; }
        public string AbsoluteProgressToNextLevel { get; set; }
        public string CombinedCssClassName { get; set; }
        public string[] AllNames { get; set; }
        public object RootLevelIconUrl { get; set; }
    }

    public enum RelationShipStatusEnum : short
    {
        None = 0,
        Requested = 1,
        BeFriend = 2,
        UnFriend = 3,
        Block = 4
    }

    public enum FriendStatus
    {
        Unknown = 0,
        Friended = 1,
        Requested = 2,
        WaitingAccept = 3,
    }
}
