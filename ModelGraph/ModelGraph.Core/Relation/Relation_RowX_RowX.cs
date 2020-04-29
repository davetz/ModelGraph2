
namespace ModelGraph.Core
{
    public class Relation_RowX_RowX : RelationOf<RowX,RowX>
    {
        internal override IdKey ViKey => IdKey.RowX_RowX;

        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal Relation_RowX_RowX(RelationXDomain owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
    }
}
