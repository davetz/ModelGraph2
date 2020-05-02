
namespace ModelGraph.Core
{
    public class Relation_GraphX_QueryX : RelationOf<GraphX,QueryX>
    {
        internal override IdKey ViKey => IdKey.GraphX_QueryX;

        internal Relation_GraphX_QueryX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
