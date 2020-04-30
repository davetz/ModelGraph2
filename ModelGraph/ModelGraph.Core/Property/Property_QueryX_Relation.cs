
namespace ModelGraph.Core
{
    public class Property_QueryX_Relation : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXRelationProperty;

        internal Property_QueryX_Relation(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetQueryXRelationName(Cast(item));
    }
}
