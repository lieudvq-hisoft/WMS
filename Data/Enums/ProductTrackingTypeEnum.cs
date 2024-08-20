using System.ComponentModel;

namespace Data.Enums;

public enum ProductTrackingType
{
    [Description("By Unique Serial Number")]
    Serial,

    [Description("By Lots")]
    Lot,

    [Description("No Tracking")]
    None,
}


