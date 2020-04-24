using System;
using System.Collections.Generic;
using System.Xml;

namespace ModelGraph.Core
{/*

 */
    public partial class Chef
    {
        private Dictionary<Property, EnumZ> Property_Enum;

        private void InitializeReferences()
        {
            // static enum properties
            Property_Enum = new Dictionary<Property, EnumZ>
            {
                { ColumnXDomain.TypeOfProperty, ValueTypeEnum },

                { RelationXDomain.PairingProperty, PairingEnum },

                { GraphParams.OrientationProperty, OrientationEnum },
                { GraphParams.LabelingProperty, LabelingEnum },
                { GraphParams.ResizingProperty, ResizingEnum },
                { GraphParams.BarWidthProperty, BarWidthEnum },

                { GraphParams.Facet1Property, FacetEnum },
                { GraphParams.Facet2Property, FacetEnum },

                { SymbolXDomain.AttachProperty, AttatchEnum },

                { QueryXDomain.LineStyleProperty, LineStyleEnum },
                { QueryXDomain.DashStyleProperty, DashStyleEnum },

                { QueryXDomain.Facet1Property, FacetEnum },
                { QueryXDomain.Facet2Property, FacetEnum },

                { ComputeXDomain.CompuTypeProperty, ComputeTypeEnum },
            };
        }
    }
}
