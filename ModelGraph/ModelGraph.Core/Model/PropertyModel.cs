﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.Web.Http;

namespace ModelGraph.Core
{
    public abstract class PropertyModel : LineModel
    {
        internal Property Property;

        internal PropertyModel(LineModel owner, Item item, Property prop)
        {
            Item = item;
            Owner = owner;
            Depth = (byte)(owner.Depth + 1);

            owner.CovertInsert(this);

            Property = prop;
        }

        public bool IsReadOnly => Property.IsReadonly;
        public bool IsMultiline => Property.IsMultiline;

        public virtual bool IsTextModel => false;
        public virtual bool IsCheckModel => false;
        public virtual bool IsComboModel => false;


        public virtual int GetIndexValue(Root root) => default;
        public virtual bool GetBoolValue(Root root) => default;
        public virtual string GetTextValue(Root root) => default;
        public virtual string[] GetlListValue(Root root) => default;

        public void PostSetIndexValue(Root root, int val) { if (val != GetIndexValue(root)) root.PostSetIndexValue(Item, Property, val); }
        public void PostSetBoolValue(Root root, bool val) { if (val != GetBoolValue(root)) root.PostSetBoolValue(Item, Property, val); }
        public void PostSetTextValue(Root root, string val) { if (val != GetTextValue(root)) root.PostSetTextValue(Item, Property, val); }

        public override (string, string) GetKindNameId(Root root)
        {
            var name = Property.GetNameId(root);

            return (null, Property.HasParentName ? Property.GetParentName(root, Item) : name);
        }

        public override string GetModelIdentity() => $"{IdKey}{Environment.NewLine}{Property.IdKey}  ({Property.ItemKey:X3}";
    }
}
