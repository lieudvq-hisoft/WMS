namespace Data.Enums;

using System.ComponentModel;

public enum StockMoveState
{
    [Description("New: The stock move is created but not confirmed.")]
    Draft,

    [Description("Waiting Another Move: A linked stock move should be done before this one.")]
    Waiting,

    [Description("Waiting Availability: The stock move is confirmed but the product can't be reserved.")]
    Confirmed,

    [Description("Partially Available: The stock move is partially available.")]
    PartiallyAvailable,

    [Description("Available: The product of the stock move is reserved.")]
    Assigned,

    [Description("Done: The product has been transferred and the transfer has been confirmed.")]
    Done,

    [Description("Cancelled: The stock move has been cancelled.")]
    Cancelled
}


