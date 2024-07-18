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

public enum ProductCategorySortCriteria
{
    CompleteName,
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

public enum TrackingType
{
    [EnumMember(Value = "Serial")]
    Serial,

    [EnumMember(Value = "Lot")]
    Lot,

    [EnumMember(Value = "None")]
    None
}

public enum RemovalMethod
{
    [EnumMember(Value = "LIFO")]
    LIFO,

    [EnumMember(Value = "FIFO")]
    FIFO,
}

