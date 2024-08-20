using System.ComponentModel;

namespace Data.Enums;

public enum ProductTrackingType
{
    [Description("No Tracking")]
    None,
    [Description("By Unique Serial Number")]
    Serial,
    [Description("By Lots")]
    Lot,
}


