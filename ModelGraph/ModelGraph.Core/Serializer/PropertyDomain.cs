using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class PropertyDomain : InternalDomainOf<Property>
    {
        internal PropertyDomain(Chef owner) : base(owner, IdKey.PropertyStore, 100)
        {
        }

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.PropertyStore;
        internal override string ParentNameId => KindId;
        #endregion

    }
}
