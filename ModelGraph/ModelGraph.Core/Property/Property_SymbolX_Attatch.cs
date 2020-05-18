
namespace ModelGraph.Core
{
    public class Property_SymbolX_Attatch : PropertyOf<SymbolX, string>
    {
        internal override IdKey IdKey => IdKey.SymbolXAttatchProperty;

        internal Property_SymbolX_Attatch(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Attach>().GetEnumName(chef, (int)Cast(item).Attach); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).Attach = (Attach)chef.Get<Enum_Attach>().GetKey(chef, val); }
    }
}
