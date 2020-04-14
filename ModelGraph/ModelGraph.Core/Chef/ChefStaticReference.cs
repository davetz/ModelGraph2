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
                { ColumnXStore.TypeOfProperty, ValueTypeEnum },

                { RelationXStore.PairingProperty, PairingEnum },

                { GraphParams.OrientationProperty, OrientationEnum },
                { GraphParams.LabelingProperty, LabelingEnum },
                { GraphParams.ResizingProperty, ResizingEnum },
                { GraphParams.BarWidthProperty, BarWidthEnum },

                { GraphParams.Facet1Property, FacetEnum },
                { GraphParams.Facet2Property, FacetEnum },

                { SymbolXStore.AttachProperty, AttatchEnum },

                { QueryXStore.LineStyleProperty, LineStyleEnum },
                { QueryXStore.DashStyleProperty, DashStyleEnum },

                { QueryXStore.Facet1Property, FacetEnum },
                { QueryXStore.Facet2Property, FacetEnum },

                { ComputeXStore.CompuTypeProperty, ComputeTypeEnum },
            };
        }
    }
}
