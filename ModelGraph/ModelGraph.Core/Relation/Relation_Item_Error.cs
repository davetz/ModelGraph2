
namespace ModelGraph.Core
{
    public class Relation_Item_Error : RelationOf<Item,Error>
    {
        internal override IdKey ViKey => IdKey.Item_Error;

        internal Relation_Item_Error(RelationZStore owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
