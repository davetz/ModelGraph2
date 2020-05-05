﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public abstract class PropertyModel : LineModel
    {
        internal Property Property;

        internal PropertyModel(LineModel owner, Item item, Property prop) : base(owner, item)
        {            
            Property = prop;
        }

        public bool IsReadOnly => Property.IsReadonly;
        public bool IsMultiline => Property.IsMultiline;

        public virtual bool IsTextModel => false;
        public virtual bool IsCheckModel => false;
        public virtual bool IsComboModel => false;


        public virtual int GetIndexValue(Chef chef) => default;
        public virtual bool GetBoolValue(Chef chef) => default;
        public virtual string GetTextValue(Chef chef) => default;
        public virtual string[] GetlListValue(Chef chef) => default;

        public virtual void PostSetValue(Chef chef, int val) { }
        public virtual void PostSetValue(Chef chef, bool val) { }
        public virtual void PostSetValue(Chef chef, string val) { }

        internal override (bool anyChange, int flatCount) Validate() => (false, 0);

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, 0);
        }

        public override (string, string) GetKindNameId(Chef chef)
        {
            var name = Property.GetSingleNameId(chef);

            return (null, Property.HasParentName ? Property.GetParentName(chef, Item) : name);
        }

        public override string GetModelIdentity() => $"{IdKey}  ({ItemKey:X3}){Environment.NewLine}{Property.IdKey}  ({Property.ItemKey:X3})";
    }
}
