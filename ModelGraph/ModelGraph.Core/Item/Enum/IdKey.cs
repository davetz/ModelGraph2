
namespace ModelGraph.Core
{
    //=========================================================================
    // _coreResources.resw // resource keys are strings of the form  Enum.GetNames(typeof(IdKey)); with an optional suffix
    //=========================================================================
    // suffix : none : identity text string, (put all reusable text first so the nummeric enum value < 256) usage: {byte _kind; var enu = (IdKey)_kind; }  
    // suffix : "_A" : specify Excelerator Key char  (accelerator)
    // suffix : "_E" : specify event static text     (event)
    // suffix : "_F" : specify event formated text   (formated) üsed with string.Format(<localized_text>, arg0,  arg1,..);
    // suffix : "_K" : specify item kind text        (kind)
    // suffix : "_S" : specify item tooltip text     (summary)
    // suffix : "_V" : specify item description text (verbose)
    //=========================================================================
    // The IdKey enum will prevent duplicate entries and we can find all references of every item.
    // We can freely rearrange or insert new values into the IdKey enum without messing anybody up. 
    // We can store the IdKey enum value in ushort a variable or in some cases in a byte varible.
    // e.g. static byte _itemKind = (byte)IdKey.Column; var kind = Localize(((IdKey)_itemKind).ToString());
    //=========================================================================
    /// <summary>Unique resource string key</summary>
    internal enum IdKey : ushort
    {
        Unassigned,
        Invalid,
        Item,
        Name,
        Symbol,
        Column,
        Property,
        Compute,
        ValueType,
        Pairing,
        Labeling,

        #region Command  ======================================================
        EditCommand,
        ViewCommand,
        UndoCommand,
        RedoCommand,
        MergeCommand,
        InsertCommand,
        RemoveCommand,
        CreateCommand,
        RefreshCommand,
        ExpandAllCommand,
        MakeRootLinkCommand,
        MakePathHeadCommand,
        MakeGroupHeadCommand,
        MakeEgressHeadCommand,
        #endregion

        #region Store =========================================================
        EnumXStore,
        ViewXStore,
        TableXStore,
        GraphXStore,
        QueryXStore,
        ValueXStore,
        SymbolXStore,
        ColumnXStore,
        ComputeXStore,
        RelationXStore,

        PrimeStore,
        EnumZStore,
        ErrorStore,
        GroupStore,
        PropertyStore,
        RelationStore,
        PropertyZStore,
        RelationZStore,
        #endregion

        #region Item  =========================================================
        Dummy,
        NodeParm,
        DataChef,
        //=========================================
        ChangeRoot,
        ChangeSet,
        ItemUpdated,
        ItemCreated,
        ItemRemoved,
        ItemLinked,
        ItemUnlinked,
        ItemMoved,
        ItemChildMoved,
        ItemParentMoved,
        //=========================================
        // External (user-defined) item classes
        RowX,
        PairX,
        EnumX,
        ViewX,
        TableX,
        GraphX,
        QueryX,
        SymbolX,
        ColumnX,
        ComputeX,
        CommandX,
        RelationX,
        //=========================================
        // QueryX detail, used to lookup resource strings
        QueryIsCorrupt,
        QueryGraphRoot,
        QueryGraphLink,
        QueryViewRoot,
        QueryViewHead,
        QueryViewLink,
        QueryPathHead,
        QueryPathLink,
        QueryGroupHead,
        QueryGroupLink,
        QuerySegueHead,
        QuerySegueLink,
        QueryValueRoot,
        QueryValueHead,
        QueryValueLink,
        QueryNodeSymbol,
        //=========================================
        QueryWhere, // used to lookup kind resource string "Where"
        QuerySelect, // used to lookup kind resource string "Select"
        //=========================================
        Graph,
        Query,
        Level,
        Node,
        Edge,
        Open,
        //=========================================
        QueryPath,
        FlaredPath,
        ForkedPath,
        SeriesPath,
        ParallelPath,

        LinkPath, // used to lookup kind resource string "_Link"
        RadialPath, // used to lookup kind resource string "_Radial"
        #endregion

        #region Error  ========================================================
        ExportError,
        ImportError1,

        ComputeProblemRelatedWhereSelectError,
        ComputeMissingRelatedSelectError,
        ComputeUnresolvedSelectError,
        ComputeInvalidSelectError,
        ComputeCircularDependanceError,
        ComputeMissingSelectError,

        ComputeMissingRootQueryError,
        ComputeValueOverflowError,

        QueryUnresolvedWhereError,
        QueryInvalidWhereError,

        QueryUnresolvedSelectError,
        QueryInvalidSelectError,
        QueryMissingSelectError,

        QueryValueOverflowdWhereError,
        QueryValueOverflowSelectError,
        #endregion

        #region Relation  =====================================================
        Relation,
        //=========================================
        EnumX_ColumnX,
        TableX_ColumnX,
        TableX_NameProperty,
        TableX_SummaryProperty,
        TableX_ChildRelationX,
        TableX_ParentRelationX,
        //=========================================
        TableChildRelationGroup,
        TableParentRelationGroup,
        TableReverseRelationGroup,
        TableRelationGroupRelation,
        ParentRelationGroupRelation,
        ReverseRelationGroupRelation,
        //=========================================
        Item_Error,
        ViewX_ViewX,
        ViewX_QueryX,
        QueryX_ViewX,
        Property_ViewX,
        Relation_ViewX,
        ViewX_Property,
        QueryX_Property,
        //=========================================
        GraphX_SymbolX,
        SymbolX_QueryX,
        GraphX_QueryX,
        QueryX_QueryX,
        GraphX_ColorColumnX,
        GraphX_SymbolQueryX,
        //=========================================
        Store_QueryX,
        Relation_QueryX,
        //=========================================
        Store_ComputeX,
        ComputeX_QueryX,
        //=========================================
        Store_Property,
        Store_ChildRelation,
        Store_ParentRelation,
        #endregion

        #region Property  =====================================================
        ViewNameProperty,
        ViewSummaryProperty,
        IncludeItemIdentityIndexProperty,
        //=========================================
        EnumNameProperty,
        EnumSummaryProperty,
        EnumTextProperty,
        EnumValueProperty,
        //=========================================
        TableNameProperty,
        TableSummaryProperty,
        //=========================================
        ColumnNameProperty,
        ColumnSummaryProperty,
        ColumnValueTypeProperty,
        ColumnIsChoiceProperty,
        //=========================================
        RelationNameProperty,
        RelationSummaryProperty,
        RelationPairingProperty,
        RelationIsRequiredProperty,
        RelationIsReferenceProperty,
        //=========================================
        GraphNameProperty,
        GraphSummaryProperty,
        GraphTerminalLengthProperty,
        GraphTerminalSpacingProperty,
        GraphTerminalStretchProperty,
        GraphSymbolSizeProperty,
        //=========================================
        QueryXSelectProperty,
        QueryXWhereProperty,

        QueryXConnect1Property,
        QueryXConnect2Property,

        QueryXRelationProperty,
        QueryXIsReversedProperty,
        QueryXIsImmediateProperty,
        QueryXIsPersistentProperty,
        QueryXIsBreakPointProperty,
        QueryXExclusiveKeyProperty,
        QueryXAllowSelfLoopProperty,
        QueryXIsPathReversedProperty,
        QueryXIsFullTableReadProperty,
        QueryXFacet1Property,
        QueryXFacet2Property,
        ValueXWhereProperty,
        ValueXSelectProperty,
        ValueXIsReversedProperty,
        ValueXValueTypeProperty,
        QueryXLineStyleProperty,
        QueryXDashStyleProperty,
        QueryXLineColorProperty,
        //=========================================
        SymbolXNameProperty,
        SymbolXAttatchProperty,
        //=========================================
        NodeCenterXYProperty,
        NodeSizeWHProperty,
        NodeLabelingProperty,
        NodeResizingProperty,
        NodeBarWidthProperty,
        NodeOrientationProperty,
        NodeFlipRotateProperty,
        //=========================================
        EdgeFace1Property,
        EdgeFace2Property,
        EdgeFacet1Property,
        EdgeFacet2Property,
        EdgeConnect1Property,
        EdgeConnect2Property,
        //=========================================
        ComputeXNameProperty,
        ComputeXSummaryProperty,
        ComputeXCompuTypeProperty,
        ComputeXWhereProperty,
        ComputeXSelectProperty,
        ComputeXSeparatorProperty,
        ComputeXValueTypeProperty,
        ComputeXNumericSetProperty,
        ComputeXResultsProperty,
        ComputeXSortingProperty,
        ComputeXTakeSetProperty,
        ComputeXTakeLimitProperty,
        #endregion

        #region Model =========================================================
        ParmDebugListModel,
        //=====================================================================
        DataChefModel,
        TextPropertyModel,
        CheckPropertyModel,
        ComboPropertyModel,
        //=====================================================================
        ParmRootModel,
        ErrorRootModel,
        ChangeRootModel,
        MetadataRootModel,
        ModelingRootModel,
        MetaRelationListModel,
        ErrorTypeModel,
        ErrorTextModel,
        ChangeSetModel,
        ItemChangeModel,
        MetadataSubRootModel,
        ModelingSubRootModel,
        //=====================================================================
        MetaViewViewListModel,
        MetaViewViewModel,
        MetaViewQueryModel,
        MetaViewCommandModel,
        MetaViewPropertyModel,
        ViewViewListModel,
        ViewViewModel,
        ViewItemModel,
        ViewQueryModel,
        //=====================================================================
        MetaEnumListModel,
        MetaTableListModel,
        MetaGraphListModel,
        MetaSymbolListModel,
        MetaGraphParmListModel,

        TableListModel = 0x647,
        GraphListModel = 0x648,
        //=====================================================================
        MetaPairModel,
        MetaEnumModel,
        MetaTableModel,
        MetaGraphModel,
        MetaSymbolModel,
        MetaColumnModel,
        MetaComputeModel,
        SymbolEditorModel,
        //=====================================================================
        MetaColumnListModel,
        MetaChildRelationListModel,
        MetaParentRelatationListModel,
        MetaEnumPairListModel,
        MetaEnumColumnListModel,
        MetaComputeListModel,
        MetaEnumRelatedColumnModel,
        //=====================================================================
        MetaChildRelationModel,
        MetaParentRelationModel,
        MetaNameColumnRelationModel,
        MetaSummaryColumnRelationModel,
        MetaNameColumnModel,
        MetaSummaryColumnModel,
        //=====================================================================
        MetaGraphColoringModel,
        MetaGraphRootListModel,
        MetaGraphNodeListModel,
        MetaGraphNodeModel,
        MetaGraphColorColumnModel,
        //=====================================================================
        MetaGraphRootModel,
        MetaGraphLinkModel,
        MetaGraphPathHeadModel,
        MetaGraphPathLinkModel,
        MetaGraphGroupHeadModel,
        MetaGraphGroupLinkModel,
        MetaGraphEgressHeadModel,
        MetaGraphEgressLinkModel,
        MetaGraphNodeSymbolModel,

        MetaValueHeadModel,
        MetaValueLinkModel,
        //=====================================================================
        RowModel,
        TableModel,
        GraphModel,
        GraphRefModel,
        RowChildRelationModel,
        RowParentRelationModel,
        RowRelatedChildModel,
        RowRelatedParentModel,
        //=====================================================================
        RowPropertyListModel,
        RowChildRelationListModel,
        RowParentRelationListModel,
        RowDefaultPropertyListModel,
        RowUnusedChildRelationListModel,
        RowUnusedParentRelationListModel,
        RowComputeListModel,
        //=====================================================================
        QueryRootLinkModel,
        QueryPathHeadModel,
        QueryPathLinkModel,
        QueryGroupHeadModel,
        QueryGroupLinkModel,
        QueryEgressHeadModel,
        QueryEgressLinkModel,
        //=====================================================================
        QueryRootItemModel,
        QueryPathStepModel,
        QueryPathTailModel,
        QueryGroupStepModel,
        QueryGroupTailModel,
        QueryEgressStepModel,
        QueryEgressTailModel,
        //=====================================================================
        GraphXRefModel,
        GraphNodeListModel,
        GraphEdgeListModel,
        GraphRootListModel,
        GraphLevelListModel,

        GraphLevelModel,
        GraphPathModel,
        GraphRootModel,
        GraphNodeModel,
        GraphEdgeModel,

        GraphOpenListModel,
        GraphOpenModel,
        //=====================================================================
        PrimeComputeModel,
        ComputeStoreModel,
        //=====================================================================
        InternalStoreListModel,
        InternalStoreModel,

        StoreItemModel,

        StoreItemItemListModel,
        StoreRelationLinkListModel,

        StoreChildRelationListModel,
        StoreParentRelationListModel,

        StoreItemItemModel,
        StoreRelationLinkModel,

        StoreChildRelationModel,
        StoreParentRelationModel,

        StoreRelatedItemModel,
        #endregion

        #region Enum  =========================================================
        ValueTypeEnum,
        ValueType_Bool,
        ValueType_BoolArray,
        ValueType_Char,
        ValueType_CharArray,
        ValueType_Byte,
        ValueType_ByteArray,
        ValueType_SByte,
        ValueType_SByteArray,
        ValueType_Int16,
        ValueType_Int16Array,
        ValueType_UInt16,
        ValueType_UInt16Array,
        ValueType_Int32,
        ValueType_Int32Array,
        ValueType_UInt32,
        ValueType_UInt32Array,
        ValueType_Int64,
        ValueType_Int64Array,
        ValueType_UInt64,
        ValueType_UInt64Array,
        ValueType_Single,
        ValueType_SingleArray,
        ValueType_Double,
        ValueType_DoubleArray,
        ValueType_Decimal,
        ValueType_DecimalArray,
        ValueType_DateTime,
        ValueType_DateTimeArray,
        ValueType_String,
        ValueType_StringArray,

        PairingEnum,
        Pairing_OneToOne,
        Pairing_OneToMany,
        Pairing_ManyToMany,

        AspectEnum,
        Aspect_Point,
        Aspect_Square,
        Aspect_Vertical,
        Aspect_Horizontal,

        LabelingEnum,
        Labeling_None,
        Labeling_Top,
        Labeling_Left,
        Labeling_Right,
        Labeling_Bottom,
        Labeling_Center,
        Labeling_TopLeft,
        Labeling_TopRight,
        Labeling_BottomLeft,
        Labeling_BottomRight,
        Labeling_TopLeftSide,
        Labeling_TopRightSide,
        Labeling_TopLeftCorner,
        Labeling_TopRightCorner,
        Labeling_BottomLeftSide,
        Labeling_BottomRightSide,
        Labeling_BottomLeftCorner,
        Labeling_BottomRightCorner,

        FlipRotateEnum,
        FlipRotate_None,
        FlipRotate_FlipVertical,
        FlipRotate_FlipHorizontal,
        FlipRotate_FlipBothWays,
        FlipRotate_RotateClockwise,
        FlipRotate_RotateFlipVertical,
        FlipRotate_RotateFlipHorizontal,
        FlipRotate_RotateFlipBothWays,

        ResizingEnum,
        Resizing_Auto,
        Resizing_Fixed,
        Resizing_Manual,

        NamingEnum,
        Naming_None,
        Naming_Default,
        Naming_UniqueNumber,
        Naming_Alphabetic,
        Naming_SubstituteParent,

        BarWidthEnum,
        BarWidth_Thin,
        BarWidth_Wide,
        BarWidth_ExtraWide,

        ContactEnum,
        Contact_Any,
        Contact_One,
        Contact_None,

        SideEnum,
        Side_Any,
        Side_East,
        Side_West,
        Side_North,
        Side_South,

        ConnectEnum,
        Connect_Any,
        Connect_East,
        Connect_West,
        Connect_East_West,
        Connect_North,
        Connect_North_East,
        Connect_North_West,
        Connect_North_East_West,
        Connect_South,
        Connect_South_East,
        Connect_South_West,
        Connect_South_East_West,
        Connect_North_South,
        Connect_North_South_East,
        Connect_North_South_West,

        FacetEnum,
        Facet_None,
        Facet_Nubby,
        Facet_Diamond,
        Facet_InArrow,
        Facet_Force_None,
        Facet_Force_Nubby,
        Facet_Force_Diamond,
        Facet_Force_InArrow,

        CompuTypeEnum,
        CompuType_RowValue,
        CompuType_RelatedValue,
        CompuType_NumericValueSet,
        CompuType_CompositeString,
        CompuType_CompositeReversed,

        NumericSetEnum,
        NumericSet_Count,
        NumericSet_Count_Min_Max,
        NumericSet_Count_Min_Max_Sum,
        NumericSet_Count_Min_Max_Sum_Ave,
        NumericSet_Count_Min_Max_Sum_Ave_Std,

        NumericTermEnum,
        BNumericTerm_Count,
        NumericTerm_Min,
        NumericTerm_Max,
        NumericTerm_Sum,
        NumericTerm_Ave,
        NumericTerm_Std,

        StartLineEnum,
        StartLine_Flat,
        StartLine_Square,
        StartLine_Round,
        StartLine_Triangle,

        EndLineEnum,
        EndLine_Flat,
        EndLine_Square,
        EndLine_Round,
        EndLine_Triangle,

        ResultsEnum,
        Results_OneValue,
        Results_AllValues,
        Results_LimitedSet,

        SortingEnum,
        Sorting_Unsorted,
        Sorting_Ascending,
        Sorting_Descending,

        TakeSetEnum,
        TakeSet_First,
        TakeSet_Last,
        TakeSet_Both,

        AttatchEnum,
        Attatch_Normal,
        Attatch_Radial,
        Attatch_RightAngle,
        Attatch_SkewedAngle,

        LineStyleEnum,
        LineStyle_PointToPoint,
        LineStyle_SimpleSpline,
        LineStyle_DoubleSpline,

        DashStyleEnum,
        DashStyle_Solid,
        DashStyle_Dashed,
        DashStyle_Dotted,
        DashStyle_DashDot,
        DashStyle_DashDotDot,
        #endregion
    }
}
