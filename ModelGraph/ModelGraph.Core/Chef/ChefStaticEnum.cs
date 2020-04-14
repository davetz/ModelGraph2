namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal EnumZ ValueTypeEnum;
        internal EnumZ PairingEnum;

        internal EnumZ OrientationEnum;
        internal EnumZ LabelingEnum;
        internal EnumZ ResizingEnum;
        internal EnumZ BarWidthEnum;

        internal EnumZ SideEnum;
        internal EnumZ FacetEnum;
        internal EnumZ ConnectEnum;
        internal EnumZ ContactEnum;
        internal EnumZ ComputeTypeEnum;
        internal EnumZ AttatchEnum;
        internal EnumZ LineStyleEnum;
        internal EnumZ DashStyleEnum;

        private void InitializeEnums()
        {
            ValueTypeEnum = new EnumZ(EnumZStore,Trait.ValueTypeEnum);
            new PairZ(ValueTypeEnum, Trait.ValueType_Bool);
            new PairZ(ValueTypeEnum, Trait.ValueType_Char);
            new PairZ(ValueTypeEnum, Trait.ValueType_SByte);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int16);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int32);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int64);
            new PairZ(ValueTypeEnum, Trait.ValueType_Single);
            new PairZ(ValueTypeEnum, Trait.ValueType_Double);
            new PairZ(ValueTypeEnum, Trait.ValueType_Decimal);
            new PairZ(ValueTypeEnum, Trait.ValueType_DateTime);
            new PairZ(ValueTypeEnum, Trait.ValueType_String);
            new PairZ(ValueTypeEnum, Trait.ValueType_Byte);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt16);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt32);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt64);
            new PairZ(ValueTypeEnum, Trait.ValueType_BoolArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_CharArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_SByteArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int16Array);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int32Array);
            new PairZ(ValueTypeEnum, Trait.ValueType_Int64Array);
            new PairZ(ValueTypeEnum, Trait.ValueType_SingleArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_DoubleArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_DecimalArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_StringArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_ByteArray);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt16Array);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt32Array);
            new PairZ(ValueTypeEnum, Trait.ValueType_UInt64Array);

            PairingEnum = new EnumZ(EnumZStore, Trait.PairingEnum);
            new PairZ(PairingEnum, Trait.Pairing_OneToOne);
            new PairZ(PairingEnum, Trait.Pairing_OneToMany);
            new PairZ(PairingEnum, Trait.Pairing_ManyToMany);

            OrientationEnum = new EnumZ(EnumZStore, Trait.AspectEnum);
            new PairZ(OrientationEnum, Trait.Aspect_Point);
            new PairZ(OrientationEnum, Trait.Aspect_Square);
            new PairZ(OrientationEnum, Trait.Aspect_Vertical);
            new PairZ(OrientationEnum, Trait.Aspect_Horizontal);

            LabelingEnum = new EnumZ(EnumZStore, Trait.LabelingEnum);
            new PairZ(LabelingEnum, Trait.Labeling_None);
            new PairZ(LabelingEnum, Trait.Labeling_Top);
            new PairZ(LabelingEnum, Trait.Labeling_Left);
            new PairZ(LabelingEnum, Trait.Labeling_Right);
            new PairZ(LabelingEnum, Trait.Labeling_Bottom);
            new PairZ(LabelingEnum, Trait.Labeling_Center);
            new PairZ(LabelingEnum, Trait.Labeling_TopLeft);
            new PairZ(LabelingEnum, Trait.Labeling_TopRight);
            new PairZ(LabelingEnum, Trait.Labeling_BottomLeft);
            new PairZ(LabelingEnum, Trait.Labeling_BottomRight);
            new PairZ(LabelingEnum, Trait.Labeling_TopLeftSide);
            new PairZ(LabelingEnum, Trait.Labeling_TopRightSide);
            new PairZ(LabelingEnum, Trait.Labeling_TopLeftCorner);
            new PairZ(LabelingEnum, Trait.Labeling_TopRightCorner);
            new PairZ(LabelingEnum, Trait.Labeling_BottomLeftSide);
            new PairZ(LabelingEnum, Trait.Labeling_BottomRightSide);
            new PairZ(LabelingEnum, Trait.Labeling_BottomLeftCorner);
            new PairZ(LabelingEnum, Trait.Labeling_BottomRightCorner);

            ResizingEnum = new EnumZ(EnumZStore, Trait.ResizingEnum);
            new PairZ(ResizingEnum, Trait.Resizing_Auto);
            new PairZ(ResizingEnum, Trait.Resizing_Fixed);
            new PairZ(ResizingEnum, Trait.Resizing_Manual);

            BarWidthEnum = new EnumZ(EnumZStore, Trait.BarWidthEnum);
            new PairZ(BarWidthEnum, Trait.BarWidth_Thin);
            new PairZ(BarWidthEnum, Trait.BarWidth_Wide);
            new PairZ(BarWidthEnum, Trait.BarWidth_ExtraWide);

            SideEnum = new EnumZ(EnumZStore, Trait.SideEnum);
            new PairZ(SideEnum, Trait.Side_Any);
            new PairZ(SideEnum, Trait.Side_East);
            new PairZ(SideEnum, Trait.Side_West);
            new PairZ(SideEnum, Trait.Side_North);
            new PairZ(SideEnum, Trait.Side_South);

            FacetEnum = new EnumZ(EnumZStore, Trait.Facet_None);
            new PairZ(FacetEnum, Trait.Facet_None);
            new PairZ(FacetEnum, Trait.Facet_Nubby);
            new PairZ(FacetEnum, Trait.Facet_Diamond);
            new PairZ(FacetEnum, Trait.Facet_InArrow);
            new PairZ(FacetEnum, Trait.Facet_Force_None);
            new PairZ(FacetEnum, Trait.Facet_Force_Nubby);
            new PairZ(FacetEnum, Trait.Facet_Force_Diamond);
            new PairZ(FacetEnum, Trait.Facet_Force_InArrow);

            ContactEnum = new EnumZ(EnumZStore, Trait.ContactEnum);
            new PairZ(ContactEnum, Trait.Contact_Any);
            new PairZ(ContactEnum, Trait.Contact_One);
            new PairZ(ContactEnum, Trait.Contact_None);

            ConnectEnum = new EnumZ(EnumZStore, Trait.ConnectEnum);
            new PairZ(ConnectEnum, Trait.Connect_Any);
            new PairZ(ConnectEnum, Trait.Connect_East);
            new PairZ(ConnectEnum, Trait.Connect_West);
            new PairZ(ConnectEnum, Trait.Connect_North);
            new PairZ(ConnectEnum, Trait.Connect_South);
            new PairZ(ConnectEnum, Trait.Connect_East_West);
            new PairZ(ConnectEnum, Trait.Connect_North_South);
            new PairZ(ConnectEnum, Trait.Connect_North_East);
            new PairZ(ConnectEnum, Trait.Connect_North_West);
            new PairZ(ConnectEnum, Trait.Connect_North_East_West);
            new PairZ(ConnectEnum, Trait.Connect_North_South_East);
            new PairZ(ConnectEnum, Trait.Connect_North_South_West);
            new PairZ(ConnectEnum, Trait.Connect_South_East);
            new PairZ(ConnectEnum, Trait.Connect_South_West);
            new PairZ(ConnectEnum, Trait.Connect_South_East_West);

            AttatchEnum = new EnumZ(EnumZStore, Trait.AttatchEnum);
            new PairZ(AttatchEnum, Trait.Attatch_Normal);
            new PairZ(AttatchEnum, Trait.Attatch_Radial);
            new PairZ(AttatchEnum, Trait.Attatch_RightAngle);
            new PairZ(AttatchEnum, Trait.Attatch_SkewedAngle);

            LineStyleEnum = new EnumZ(EnumZStore, Trait.LineStyleEnum);
            new PairZ(LineStyleEnum, Trait.LineStyle_PointToPoint);
            new PairZ(LineStyleEnum, Trait.LineStyle_SimpleSpline);
            new PairZ(LineStyleEnum, Trait.LineStyle_DoubleSpline);

            DashStyleEnum = new EnumZ(EnumZStore, Trait.DashStyleEnum);
            new PairZ(DashStyleEnum, Trait.DashStyle_Solid);
            new PairZ(DashStyleEnum, Trait.DashStyle_Dashed);
            new PairZ(DashStyleEnum, Trait.DashStyle_Dotted);
            new PairZ(DashStyleEnum, Trait.DashStyle_DashDot);
            new PairZ(DashStyleEnum, Trait.DashStyle_DashDotDot);

            ComputeTypeEnum = new EnumZ(EnumZStore, Trait.CompuTypeEnum);
            new PairZ(ComputeTypeEnum, Trait.CompuType_RowValue);
            new PairZ(ComputeTypeEnum, Trait.CompuType_RelatedValue);
            new PairZ(ComputeTypeEnum, Trait.CompuType_CompositeString);
            new PairZ(ComputeTypeEnum, Trait.CompuType_CompositeReversed);
        }

        private void ReleaseEnums()
        {
            ValueTypeEnum = null;
            PairingEnum = null;

            OrientationEnum = null;
            LabelingEnum = null;
            ResizingEnum = null;
            BarWidthEnum = null;

            SideEnum = null;
            FacetEnum = null;
            ConnectEnum = null;
            ContactEnum = null;
            ComputeTypeEnum = null;
            AttatchEnum = null;
            LineStyleEnum = null;
            DashStyleEnum = null;
        }
    }
}
