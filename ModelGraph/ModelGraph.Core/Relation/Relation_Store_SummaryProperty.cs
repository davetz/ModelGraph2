
namespace ModelGraph.Core
{
    public class Relation_Store_SummaryProperty : RelationOf<Store,Property>
    {
        internal override IdKey ViKey => IdKey.Store_SummaryProperty;

        internal Relation_Store_SummaryProperty(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
