
namespace ModelGraph.Core
{
    public class Property_Node_SizeWH : PropertyOf<Node, int[]>
    {
        internal override IdKey ViKey => IdKey.NodeSizeWHProperty;

        internal Property_Node_SizeWH(PropertyDomain owner)
        {
            Owner = owner;
            Value = new Int32ArrayValue(this);

            owner.Add(this);
        }

        internal override int[] GetValue(Chef chef, Item item) => Cast(item).SizeWH;
        internal override void SetValue(Chef chef, Item item, int[] val) => Cast(item).SizeWH = val;
    }
}
