using System.ComponentModel;

namespace Data.Enums;

public enum LocationType
{
    [Description("View: Virtual location used to create a hierarchical structures for your warehouse, aggregating its child locations ; can't directly contain products")]
    View,

    [Description("Internal Location: Physical locations inside your own warehouses")]
    Internal,

    [Description("Inventory Loss: Virtual location serving as counterpart for inventory operations used to correct stock levels (Physical inventories)")]
    Inventory,
}


