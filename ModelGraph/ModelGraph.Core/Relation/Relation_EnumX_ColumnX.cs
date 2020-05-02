
namespace ModelGraph.Core
{
    public class Relation_EnumX_ColumnX : RelationOf<EnumX,ColumnX>
    {
        internal override IdKey ViKey => IdKey.EnumX_ColumnX;

        internal Relation_EnumX_ColumnX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
