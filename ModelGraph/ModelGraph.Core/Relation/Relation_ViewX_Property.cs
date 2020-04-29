
namespace ModelGraph.Core
{
    public class Relation_ViewX_Property : RelationOf<ViewX,Property>
    {
        internal override IdKey ViKey => IdKey.ViewX_Property;

        internal Relation_ViewX_Property(RelationDomain owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
