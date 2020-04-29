
namespace ModelGraph.Core
{
    public class Relation_GraphX_SymbolX : RelationOf<GraphX,SymbolX>
    {
        internal override IdKey ViKey => IdKey.GraphX_SymbolX;

        internal Relation_GraphX_SymbolX(RelationDomain owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
