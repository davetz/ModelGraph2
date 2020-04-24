using System;

namespace ModelGraph.Core
{
    public class Item
    {
        internal Item Owner;        //each item has an owner, this-> owner-> ... -> dataChef

        internal IdKey OldIdKey;       //identity, static flags, and resource string key
        private State _state;       //bit flags specific to each item type

        private byte _flags;        //IsNew, IsDeleted, AutoExpandLeft, AutoExpandRight,..
        internal byte ModelDelta;   //incremented when a property or relation is changed
        internal byte ChildDelta;   //incremented when list of child items is changed 
        internal byte ErrorDelta;   //incremented when item's error state has changed

        #region Identity  =====================================================
        internal virtual IdKey VKey => OldIdKey;
        internal virtual string Name { get => "??"; set => _ = value; }
        internal virtual string Summary { get => ""; set => _ = value; }
        internal virtual string Description { get => ""; set => _ = value; }

        internal virtual string GetKindId(Chef chef) => chef.GetKindId(VKey);
        internal virtual string GetSingleNameId(Chef chef) => GetChef().GetSingleNameId(VKey);
        internal virtual string GetParentNameId(Chef chef) => Owner.GetSingleNameId(chef);
        internal virtual string GetDoubleNameId(Chef chef) => $"{GetParentNameId(chef)} : {GetSingleNameId(chef)}";
        internal virtual string GetChangeLogId(Chef chef) => GetDoubleNameId(chef);
        internal virtual (string, string) GetKindNameId(Chef chef) => (GetKindId(chef), GetSingleNameId(chef));
        internal virtual string GetSummaryId(Chef chef) => chef.GetSummaryId(VKey);
        internal virtual string GetDescriptionId(Chef chef) => chef.GetDescriptionId(VKey);
        #endregion

        #region IdKey  ========================================================
        //internal bool IsExternal => (IdKey & IdKey.IsExternal) != 0;
        //internal bool IsInternal => (IdKey & IdKey.IsInternal) != 0;

        internal bool IsDataChef => (OldIdKey == IdKey.DataChef);
        internal bool IsViewX => (OldIdKey == IdKey.ViewX);
        internal bool IsPairX => (OldIdKey == IdKey.PairX);
        internal bool IsRowX => (OldIdKey == IdKey.RowX);
        internal bool IsEnumX => (OldIdKey == IdKey.EnumX);
        internal bool IsTableX => (OldIdKey == IdKey.TableX);
        internal bool IsGraphX => (OldIdKey == IdKey.GraphX);
        internal bool IsQueryX => (OldIdKey == IdKey.QueryX);
        internal bool IsSymbolX => (OldIdKey == IdKey.SymbolX);
        internal bool IsColumnX => (OldIdKey == IdKey.ColumnX);
        internal bool IsComputeX => (OldIdKey == IdKey.ComputeX);
        //internal bool IsCommandX => (IdKey == IdKey.CommandX);
        internal bool IsRelationX => (OldIdKey == IdKey.RelationX);
        internal bool IsGraph => (OldIdKey == IdKey.Graph);
        internal bool IsNode => (OldIdKey == IdKey.Node);
        internal bool IsEdge => (OldIdKey == IdKey.Edge);

        internal bool IsItemMoved => OldIdKey == IdKey.ItemMoved;
        internal bool IsItemCreated => OldIdKey == IdKey.ItemCreated;
        internal bool IsItemUpdated => OldIdKey == IdKey.ItemUpdated;
        internal bool IsItemRemoved => OldIdKey == IdKey.ItemRemoved;
        internal bool IsItemLinked => OldIdKey == IdKey.ItemLinked;
        internal bool IsItemUnlinked => OldIdKey == IdKey.ItemUnlinked;
        internal bool IsItemLinkMoved => OldIdKey == IdKey.ItemChildMoved;


        internal bool IsExternal => (OldIdKey & IdKey.IsExternal) != 0;
        internal bool IsReference => (OldIdKey & IdKey.IsReference) != 0;
        internal bool IsCovert => (OldIdKey & IdKey.SubMask) == IdKey.IsCovert;
        internal bool IsReadOnly => (OldIdKey & IdKey.SubMask) == IdKey.IsReadOnly;
        internal bool CanMultiline => (OldIdKey & IdKey.SubMask) == IdKey.CanMultiline;

        internal int ItemKey => (int)(OldIdKey & IdKey.KeyMask);

        internal byte TraitIndex => (byte)(OldIdKey & IdKey.IndexMask);
        internal byte TraitIndexOf(IdKey idKe) => (byte)(idKe & IdKey.IndexMask);
        internal bool IsErrorAux => (OldIdKey & IdKey.IsErrorAux) != 0;
        internal bool IsErrorAux1 => (OldIdKey & IdKey.IsErrorAux1) != 0;
        internal bool IsErrorAux2 => (OldIdKey & IdKey.IsErrorAux2) != 0;
        #endregion

        #region State  ========================================================
        private bool GetFlag(State flag) => (_state & flag) != 0;
        private void SetFlag(State flag, bool value = true) { if (value) _state |= flag; else _state &= ~flag; }

        internal bool HasState() => _state != 0;
        internal ushort GetState() => (ushort)_state;
        internal void SetState(ushort state)  => _state = (State)state;

        internal QueryType QueryKind { get { return (QueryType)(_state & State.Index); } set { _state = ((_state & ~State.Index) | (State)(value)); } }

        internal bool IsHead { get { return GetFlag(State.IsHead); } set { SetFlag(State.IsHead, value); } }
        internal bool IsTail { get { return GetFlag(State.IsTail); } set { SetFlag(State.IsTail, value); } }
        internal bool IsRoot { get { return GetFlag(State.IsRoot); } set { SetFlag(State.IsRoot, value); } }
        internal bool IsPath => (QueryKind == QueryType.Path);
        internal bool IsGroup => (QueryKind == QueryType.Group);
        internal bool IsSegue => (QueryKind == QueryType.Egress);
        internal bool IsValue => (QueryKind == QueryType.Value);
        internal bool IsReversed { get { return GetFlag(State.IsReversed); } set { SetFlag(State.IsReversed, value); } }
        internal bool IsRadial { get { return GetFlag(State.IsRadial); } set { SetFlag(State.IsRadial, value); } }

        internal bool IsBreakPoint { get { return GetFlag(State.IsBreakPoint); } set { SetFlag(State.IsBreakPoint, value); } }
        internal bool IsPersistent { get { return GetFlag(State.IsPersistent); } set { SetFlag(State.IsPersistent, value); } }

        internal bool IsLimited { get { return GetFlag(State.IsLimited); } set { SetFlag(State.IsLimited, value); } }
        internal bool IsRequired { get { return GetFlag(State.IsRequired); } set { SetFlag(State.IsRequired, value); } }

        internal bool IsUndone { get { return GetFlag(State.IsUndone); } set { SetFlag(State.IsUndone, value); } }
        internal bool IsVirgin { get { return GetFlag(State.IsVirgin); } set { SetFlag(State.IsVirgin, value); } }
        internal bool IsCongealed { get { return GetFlag(State.IsCongealed); } set { SetFlag(State.IsCongealed, value); } }

        internal bool IsChoice { get { return GetFlag(State.IsChoice); } set { SetFlag(State.IsChoice, value); } }
        internal bool NeedsRefresh { get { return GetFlag(State.NeedsRefresh); } set { SetFlag(State.NeedsRefresh, value); } }

        internal bool IsQueryGraphLink => !IsRoot && QueryKind == QueryType.Graph;
        internal bool IsQueryGraphRoot => IsRoot && QueryKind == QueryType.Graph;

        internal bool IsValueXHead => IsHead && QueryKind == QueryType.Value;// IdKey == IdKey.QueryXValueHead;

        internal bool IsGraphLink => (!IsRoot && QueryKind == QueryType.Graph);
        internal bool IsPathHead => IsHead && QueryKind == QueryType.Path;
        internal bool IsGroupHead => IsHead && QueryKind == QueryType.Group;
        internal bool IsSegueHead => IsHead && QueryKind == QueryType.Egress;

        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

        internal bool IsNew { get { return (_flags & B1) != 0; } set { _flags = value ? (byte)(_flags | B1) : (byte)(_flags & ~B1); } }
        internal bool IsDeleted { get { return (_flags & B2) != 0; } set { _flags = value ? (byte)(_flags | B2) : (byte)(_flags & ~B2); } }
        internal bool AutoExpandLeft { get { return (_flags & B3) != 0; } set { _flags = value ? (byte)(_flags | B3) : (byte)(_flags & ~B3); } }
        internal bool AutoExpandRight { get { return (_flags & B4) != 0; } set { _flags = value ? (byte)(_flags | B4) : (byte)(_flags & ~B4); } }
        #endregion

        #region StringKeys  ===================================================
        internal string KindKey => GetKindKey(OldIdKey);
        internal string NameKey => GetNameKey(OldIdKey);
        internal string SummaryKey => GetSummaryKey(OldIdKey);
        internal string DescriptionKey => GetDescriptionKey(OldIdKey);

        internal string GetKindKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}K";
        internal string GetNameKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}N";
        internal string GetSummaryKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}S";
        internal string GetDescriptionKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}V";
        internal string GetAcceleratorKey(IdKey idKe) => $"{(int)(idKe & IdKey.KeyMask):X3}A".ToUpper();
        #endregion

        #region Property/Methods ==============================================
        internal int Index => (Owner is Store st) ? st.IndexOf(this) : -1;
        internal bool IsInvalid => IsDeleted;
        internal bool IsValid => !IsInvalid;

        internal Store Store => Owner as Store;
        /// <summary>
        /// Walk up item tree hierachy to find the parent DataChef
        /// </summary>
        internal Chef GetChef()
        {
            var item = this;
            while (item != null) { if (item.IsDataChef) return item as Chef; item = item.Owner; }
            throw new Exception("GetChef: Corrupted item hierarchy"); // I seriously hope this never happens
        }
        #endregion


        #region Flags  ========================================================
        // don't read/write missing or default-value propties
        // these flags indicate which properties were non-default
        public const byte BZ = 0;
        public const byte B1 = 0x1;
        public const byte B2 = 0x2;
        public const byte B3 = 0x4;
        public const byte B4 = 0x8;
        public const byte B5 = 0x10;
        public const byte B6 = 0x20;
        public const byte B7 = 0x40;
        public const byte B8 = 0x80;

        public const ushort SZ = 0;
        public const ushort S1 = 0x1;
        public const ushort S2 = 0x2;
        public const ushort S3 = 0x4;
        public const ushort S4 = 0x8;
        public const ushort S5 = 0x10;
        public const ushort S6 = 0x20;
        public const ushort S7 = 0x40;
        public const ushort S8 = 0x80;
        public const ushort S9 = 0x100;
        public const ushort S10 = 0x200;
        public const ushort S11 = 0x400;
        public const ushort S12 = 0x800;
        public const ushort S13 = 0x1000;
        public const ushort S14 = 0x2000;
        public const ushort S15 = 0x4000;
        public const ushort S16 = 0x8000;
        #endregion
    }
}
