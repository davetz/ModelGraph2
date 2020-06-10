﻿
namespace ModelGraph.Core
{
    public class Property_Node_BarWidth : PropertyOf<Node, string>
    {
        internal override IdKey IdKey => IdKey.NodeBarWidthProperty;

        internal Property_Node_BarWidth(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.Get<Enum_BarWidth>().GetEnumName(root, (int)Cast(item).BarWidth); }
        internal override void SetValue(Item item, string val) { var root = DataChef; Cast(item).BarWidth = (BarWidth)root.Get<Enum_BarWidth>().GetKey(root, val); }
    }
}
