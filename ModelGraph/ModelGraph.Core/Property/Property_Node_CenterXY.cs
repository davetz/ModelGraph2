
namespace ModelGraph.Core
{
    public class Property_Node_CenterXY : PropertyOf<Node, int[]>
    {
        internal override IdKey ViKey => IdKey.NodeCenterXYProperty;

        internal Property_Node_CenterXY(PropertyDomain owner)
        {
            Owner = owner;
            Value = new Int32ArrayValue(this);

            owner.Add(this);
        }

        internal override int[] GetValue(Chef chef, Item item) => Cast(item).CenterXY;
        internal override void SetValue(Chef chef, Item item, int[] val) => Cast(item).CenterXY = val;
    }
}
