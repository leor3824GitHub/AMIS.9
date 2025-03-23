using AMIS.Framework.Core.Identity.Users.Dtos;

namespace AMIS.Framework.Core.Identity.Users.Features.AssignUserRole;
public class AssignUserRoleCommand
{
    public List<UserRoleDetail> UserRoles { get; set; } = new();
}
