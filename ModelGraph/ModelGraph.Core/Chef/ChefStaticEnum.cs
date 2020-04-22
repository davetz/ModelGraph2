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
            ValueTypeEnum = new EnumZ(EnumZStore,IdKey.ValueTypeEnum);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Bool);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Char);
            new PairZ(ValueTypeEnum, IdKey.ValueType_SByte);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int16);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int32);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int64);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Single);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Double);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Decimal);
            new PairZ(ValueTypeEnum, IdKey.ValueType_DateTime);
            new PairZ(ValueTypeEnum, IdKey.ValueType_String);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Byte);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt16);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt32);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt64);
            new PairZ(ValueTypeEnum, IdKey.ValueType_BoolArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_CharArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_SByteArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int16Array);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int32Array);
            new PairZ(ValueTypeEnum, IdKey.ValueType_Int64Array);
            new PairZ(ValueTypeEnum, IdKey.ValueType_SingleArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_DoubleArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_DecimalArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_StringArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_ByteArray);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt16Array);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt32Array);
            new PairZ(ValueTypeEnum, IdKey.ValueType_UInt64Array);

            PairingEnum = new EnumZ(EnumZStore, IdKey.PairingEnum);
            new PairZ(PairingEnum, IdKey.Pairing_OneToOne);
            new PairZ(PairingEnum, IdKey.Pairing_OneToMany);
            new PairZ(PairingEnum, IdKey.Pairing_ManyToMany);

            OrientationEnum = new EnumZ(EnumZStore, IdKey.AspectEnum);
            new PairZ(OrientationEnum, IdKey.Aspect_Point);
            new PairZ(OrientationEnum, IdKey.Aspect_Square);
            new PairZ(OrientationEnum, IdKey.Aspect_Vertical);
            new PairZ(OrientationEnum, IdKey.Aspect_Horizontal);

            LabelingEnum = new EnumZ(EnumZStore, IdKey.LabelingEnum);
            new PairZ(LabelingEnum, IdKey.Labeling_None);
            new PairZ(LabelingEnum, IdKey.Labeling_Top);
            new PairZ(LabelingEnum, IdKey.Labeling_Left);
            new PairZ(LabelingEnum, IdKey.Labeling_Right);
            new PairZ(LabelingEnum, IdKey.Labeling_Bottom);
            new PairZ(LabelingEnum, IdKey.Labeling_Center);
            new PairZ(LabelingEnum, IdKey.Labeling_TopLeft);
            new PairZ(LabelingEnum, IdKey.Labeling_TopRight);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomLeft);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomRight);
            new PairZ(LabelingEnum, IdKey.Labeling_TopLeftSide);
            new PairZ(LabelingEnum, IdKey.Labeling_TopRightSide);
            new PairZ(LabelingEnum, IdKey.Labeling_TopLeftCorner);
            new PairZ(LabelingEnum, IdKey.Labeling_TopRightCorner);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomLeftSide);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomRightSide);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomLeftCorner);
            new PairZ(LabelingEnum, IdKey.Labeling_BottomRightCorner);

            ResizingEnum = new EnumZ(EnumZStore, IdKey.ResizingEnum);
            new PairZ(ResizingEnum, IdKey.Resizing_Auto);
            new PairZ(ResizingEnum, IdKey.Resizing_Fixed);
            new PairZ(ResizingEnum, IdKey.Resizing_Manual);

            BarWidthEnum = new EnumZ(EnumZStore, IdKey.BarWidthEnum);
            new PairZ(BarWidthEnum, IdKey.BarWidth_Thin);
            new PairZ(BarWidthEnum, IdKey.BarWidth_Wide);
            new PairZ(BarWidthEnum, IdKey.BarWidth_ExtraWide);

            SideEnum = new EnumZ(EnumZStore, IdKey.SideEnum);
            new PairZ(SideEnum, IdKey.Side_Any);
            new PairZ(SideEnum, IdKey.Side_East);
            new PairZ(SideEnum, IdKey.Side_West);
            new PairZ(SideEnum, IdKey.Side_North);
            new PairZ(SideEnum, IdKey.Side_South);

            FacetEnum = new EnumZ(EnumZStore, IdKey.Facet_None);
            new PairZ(FacetEnum, IdKey.Facet_None);
            new PairZ(FacetEnum, IdKey.Facet_Nubby);
            new PairZ(FacetEnum, IdKey.Facet_Diamond);
            new PairZ(FacetEnum, IdKey.Facet_InArrow);
            new PairZ(FacetEnum, IdKey.Facet_Force_None);
            new PairZ(FacetEnum, IdKey.Facet_Force_Nubby);
            new PairZ(FacetEnum, IdKey.Facet_Force_Diamond);
            new PairZ(FacetEnum, IdKey.Facet_Force_InArrow);

            ContactEnum = new EnumZ(EnumZStore, IdKey.ContactEnum);
            new PairZ(ContactEnum, IdKey.Contact_Any);
            new PairZ(ContactEnum, IdKey.Contact_One);
            new PairZ(ContactEnum, IdKey.Contact_None);

            ConnectEnum = new EnumZ(EnumZStore, IdKey.ConnectEnum);
            new PairZ(ConnectEnum, IdKey.Connect_Any);
            new PairZ(ConnectEnum, IdKey.Connect_East);
            new PairZ(ConnectEnum, IdKey.Connect_West);
            new PairZ(ConnectEnum, IdKey.Connect_North);
            new PairZ(ConnectEnum, IdKey.Connect_South);
            new PairZ(ConnectEnum, IdKey.Connect_East_West);
            new PairZ(ConnectEnum, IdKey.Connect_North_South);
            new PairZ(ConnectEnum, IdKey.Connect_North_East);
            new PairZ(ConnectEnum, IdKey.Connect_North_West);
            new PairZ(ConnectEnum, IdKey.Connect_North_East_West);
            new PairZ(ConnectEnum, IdKey.Connect_North_South_East);
            new PairZ(ConnectEnum, IdKey.Connect_North_South_West);
            new PairZ(ConnectEnum, IdKey.Connect_South_East);
            new PairZ(ConnectEnum, IdKey.Connect_South_West);
            new PairZ(ConnectEnum, IdKey.Connect_South_East_West);

            AttatchEnum = new EnumZ(EnumZStore, IdKey.AttatchEnum);
            new PairZ(AttatchEnum, IdKey.Attatch_Normal);
            new PairZ(AttatchEnum, IdKey.Attatch_Radial);
            new PairZ(AttatchEnum, IdKey.Attatch_RightAngle);
            new PairZ(AttatchEnum, IdKey.Attatch_SkewedAngle);

            LineStyleEnum = new EnumZ(EnumZStore, IdKey.LineStyleEnum);
            new PairZ(LineStyleEnum, IdKey.LineStyle_PointToPoint);
            new PairZ(LineStyleEnum, IdKey.LineStyle_SimpleSpline);
            new PairZ(LineStyleEnum, IdKey.LineStyle_DoubleSpline);

            DashStyleEnum = new EnumZ(EnumZStore, IdKey.DashStyleEnum);
            new PairZ(DashStyleEnum, IdKey.DashStyle_Solid);
            new PairZ(DashStyleEnum, IdKey.DashStyle_Dashed);
            new PairZ(DashStyleEnum, IdKey.DashStyle_Dotted);
            new PairZ(DashStyleEnum, IdKey.DashStyle_DashDot);
            new PairZ(DashStyleEnum, IdKey.DashStyle_DashDotDot);

            ComputeTypeEnum = new EnumZ(EnumZStore, IdKey.CompuTypeEnum);
            new PairZ(ComputeTypeEnum, IdKey.CompuType_RowValue);
            new PairZ(ComputeTypeEnum, IdKey.CompuType_RelatedValue);
            new PairZ(ComputeTypeEnum, IdKey.CompuType_CompositeString);
            new PairZ(ComputeTypeEnum, IdKey.CompuType_CompositeReversed);
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
