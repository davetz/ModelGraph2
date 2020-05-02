
namespace ModelGraph.Core
{
    public class Relation_Property_ViewX : RelationOf<Property,ViewX>
    {
        internal override IdKey ViKey => IdKey.Property_ViewX;

        internal Relation_Property_ViewX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
