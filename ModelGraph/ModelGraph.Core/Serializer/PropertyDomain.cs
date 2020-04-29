using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class PropertyDomain : InternalDomainOf<Property>
    {
        internal PropertyDomain(Chef owner) : base(owner, IdKey.PropertyDomain, 100)
        {
        }

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.PropertyDomain;
        public override string GetParentNameId(Chef chef) => GetKindId(chef);
        #endregion

    }
}
