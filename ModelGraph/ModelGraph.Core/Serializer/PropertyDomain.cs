using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class PropertyDomain : InternalDomainOf<Property>
    {
        internal PropertyDomain(Chef chef)
        {
            Owner = chef;

            SetCapacity(100);

            chef.Add(this);
        }

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.PropertyDomain;
        public override string GetParentNameId(Chef chef) => GetKindId(chef);
        #endregion

    }
}
