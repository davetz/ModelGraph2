
namespace ModelGraph.Core
{
    public class Relation_QueryX_Property : RelationOf<QueryX,Property>
    {
        internal override IdKey ViKey => IdKey.QueryX_Property;

        internal Relation_QueryX_Property(RelationDomain owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
