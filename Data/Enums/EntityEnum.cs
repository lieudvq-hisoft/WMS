﻿using Newtonsoft.Json.Linq;
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

public enum SortUomUomCriteria
{
    CreateDate
}

public enum ProductCategorySortCriteria
{
    CompleteName,
    Name
}

public enum ProductTemplateAttributeLineSortCriteria
{
    CreateDate
}

public enum ProductVariantSortCriteria
{
    CreateDate
}

public enum StockWarehouseSortCriteria
{
    Name,
    CreateDate
}

public enum StockLocationSortCriteria
{
    Name,
    CompleteName
}

public enum SortStockPickingTypeCriteria
{
    Barcode
}

public enum StockQuantSortCriteria
{
    CreateDate
}

public enum StockMoveLineSortCriteria
{
    CreateDate
}

public enum SortStockPickingCriteria
{
    CreateDate
}

public enum StockMoveSortCriteria
{
    CreateDate
}

public enum StockLotSortCriteria
{
    CreateDate,
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

