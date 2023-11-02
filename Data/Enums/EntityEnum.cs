using System;
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

public enum SupplierSortCriteria
{
    DateCreated
}

public enum ReceiptSortCriteria
{
    DateCreated

}

public enum ProductSortCriteria
{
    DateCreated

}

public enum LocationSortCriteria
{
    DateCreated

}

public enum InboundProductSortCriteria
{
    DateCreated

}

public enum InboundProductStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Cancel = 3,
}

public enum OutboundProductStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Cancel = 3,
}

public enum PickingRequestStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Cancel = 3,
}

public enum ReceiptStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Cancel = 3,
}

public enum InventoryType
{
    In = 0,
    Out = 1,
}

