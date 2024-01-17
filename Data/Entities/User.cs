using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? UserAva { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; } = true;
    public int CurrenNoticeCount { get; set; } = 0;
    public string? FcmToken { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateUpdated { get; set; } = DateTime.Now;
    public Guid? roleID { get; set; }
    [ForeignKey("roleID")]
    public virtual Role? Role { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    public static implicit operator User(Guid v)
    {
        throw new NotImplementedException();
    }
}
