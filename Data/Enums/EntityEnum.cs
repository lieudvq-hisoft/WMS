using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Data.Enums;

public enum RoleType
{
    Admin = 1,
    Member = 2,
}
public enum NotificationSortCriteria
{
    DateCreated
}
public enum UserSortCriteria
{
    Email
}

public enum SortCriteria
{
    Name
}

public enum UomType
{
    [EnumMember(Value = "Bigger")]
    Bigger,

    [EnumMember(Value = "Reference")]
    Reference,

    [EnumMember(Value = "Smaller")]
    Smaller
}

