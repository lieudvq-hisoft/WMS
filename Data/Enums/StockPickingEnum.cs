namespace Data.Enums;

using System.ComponentModel;

public enum PickingState
{
    [Description("Draft: The transfer is not confirmed yet. Reservation doesn't apply.")]
    Draft,

    [Description("Waiting Another Operation: This transfer is waiting for another operation before being ready.")]
    Waiting,

    [Description("Waiting: The transfer is waiting for the availability of some products.\n(a) The shipping policy is \"As soon as possible\": no product could be reserved.\n(b) The shipping policy is \"When all products are ready\": not all the products could be reserved.")]
    Confirmed,

    [Description("Ready: The transfer is ready to be processed.\n(a) The shipping policy is \"As soon as possible\": at least one product has been reserved.\n(b) The shipping policy is \"When all products are ready\": all product have been reserved.")]
    Assigned,

    [Description("Done: The transfer has been processed.")]
    Done,

    [Description("Cancelled: The transfer has been cancelled.")]
    Cancelled
}


