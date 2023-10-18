using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model;

public class RoleModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public bool IsDeactive { get; set; }
}
