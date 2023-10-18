using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public bool IsDeactive { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}
