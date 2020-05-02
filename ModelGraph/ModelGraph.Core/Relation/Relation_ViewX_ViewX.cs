
namespace ModelGraph.Core
{
    public class Relation_ViewX_ViewX : RelationOf<ViewX,ViewX>
    {
        internal override IdKey ViKey => IdKey.ViewX_ViewX;

        internal Relation_ViewX_ViewX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
