
using System;

namespace ModelGraph.Core
{
    public class Property_ColumnX_ValueType : EnumZProperty
    {
        internal override IdKey IdKey => IdKey.ColumnValueTypeProperty;

        internal Property_ColumnX_ValueType(StoreOf_Property owner) : base(owner, owner.DataChef.Get<Enum_ValueType>()) { }

        internal override int GetItemPropertyValue(Item item) => (int)Cast(item).Value.ValType;

        internal override void SetItemPropertyValue(Item item, int val)
        {
            var col = Cast(item);
            var chef = DataChef;

            if (val < 0 || val >= (int)ValType.MaximumType) return;

            var type = (ValType)val;
            if (col.Value.ValType == type) return;

            var newGroup = Value.GetValGroup(type);
            var preGroup = Value.GetValGroup(col.Value.ValType);

            if (!chef.Get<Relation_Store_ColumnX>().TryGetParent(col, out Store tbl)) return;

            var N = tbl.Count;

            if (N == 0)
            {
                col.Value = Value.Create(type); //no existing values so nothing to convert
                return;
            }

            //=================================================================
            // convert the existing values that are of a different data type
            //=================================================================

            if ((newGroup & ValGroup.ScalarGroup) != 0 && (preGroup & ValGroup.ScalarGroup) != 0)
            {
                var rows = tbl.GetItems();
                var value = Value.Create(type, N);

                switch (newGroup)
                {
                    case ValGroup.Bool:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            col.Value.GetValue(key, out bool v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.Long:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            col.Value.GetValue(key, out Int64 v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.String:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            col.Value.GetValue(key, out string v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    case ValGroup.Double:
                        for (int i = 0; i < N; i++)
                        {
                            var key = rows[i];
                            col.Value.GetValue(key, out double v);
                            if (!value.SetValue(key, v)) return;
                        }
                        break;
                    default:
                        return;
                }
                col.Value = value;
                return;
            }
            return;
        }
    }
}
