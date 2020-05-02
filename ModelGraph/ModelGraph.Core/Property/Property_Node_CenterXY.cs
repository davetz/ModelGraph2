
namespace ModelGraph.Core
{
    public class Property_Node_CenterXY : PropertyOf<Node, int[]>
    {
        internal override IdKey ViKey => IdKey.NodeCenterXYProperty;

        internal Property_Node_CenterXY(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new Int32ArrayValue(this);

            owner.Add(this);
        }

        internal override int[] GetValue(Item item) => Cast(item).CenterXY;
        internal override void SetValue(Item item, int[] val) => Cast(item).CenterXY = val;
    }
}
