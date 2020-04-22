using System;

namespace ModelGraph.Core
{
    public class Item
    {
        internal Item Owner;        //each item has an owner, this-> owner-> ... -> dataChef

        internal IdKey IdKey;       //identity, static flags, and resource string key
        private State _state;       //bit flags specific to each item type

        private byte _flags;        //IsNew, IsDeleted, AutoExpandLeft, AutoExpandRight,..
        internal byte ModelDelta;   //incremented when a property or relation is changed
        internal byte ChildDelta;   //incremented when list of child items is changed 
        internal byte ErrorDelta;   //incremented when item's error state has changed

        #region IdKey  ========================================================
        //internal bool IsExternal => (IdKey & IdKey.IsExternal) != 0;
        //internal bool IsInternal => (IdKey & IdKey.IsInternal) != 0;

        internal bool IsDataChef => (IdKey == IdKey.DataChef);
        internal bool IsViewX => (IdKey == IdKey.ViewX);
        internal bool IsPairX => (IdKey == IdKey.PairX);
        internal bool IsRowX => (IdKey == IdKey.RowX);
        internal bool IsEnumX => (IdKey == IdKey.EnumX);
        internal bool IsTableX => (IdKey == IdKey.TableX);
        internal bool IsGraphX => (IdKey == IdKey.GraphX);
        internal bool IsQueryX => (IdKey == IdKey.QueryX);
        internal bool IsSymbolX => (IdKey == IdKey.SymbolX);
        internal bool IsColumnX => (IdKey == IdKey.ColumnX);
        internal bool IsComputeX => (IdKey == IdKey.ComputeX);
        //internal bool IsCommandX => (IdKey == IdKey.CommandX);
        internal bool IsRelationX => (IdKey == IdKey.RelationX);
        internal bool IsGraph => (IdKey == IdKey.Graph);
        internal bool IsNode => (IdKey == IdKey.Node);
        internal bool IsEdge => (IdKey == IdKey.Edge);

        internal bool IsItemMoved => IdKey == IdKey.ItemMoved;
        internal bool IsItemCreated => IdKey == IdKey.ItemCreated;
        internal bool IsItemUpdated => IdKey == IdKey.ItemUpdated;
        internal bool IsItemRemoved => IdKey == IdKey.ItemRemoved;
        internal bool IsItemLinked => IdKey == IdKey.ItemLinked;
        internal bool IsItemUnlinked => IdKey == IdKey.ItemUnlinked;
        internal bool IsItemLinkMoved => IdKey == IdKey.ItemChildMoved;


        internal bool IsExternal => (IdKey & IdKey.IsExternal) != 0;
        internal bool IsReference => (IdKey & IdKey.IsReference) != 0;
        internal bool IsCovert => (IdKey & IdKey.SubMask) == IdKey.IsCovert;
        internal bool IsReadOnly => (IdKey & IdKey.SubMask) == IdKey.IsReadOnly;
        internal bool CanMultiline => (IdKey & IdKey.SubMask) == IdKey.CanMultiline;

        internal int ItemKey => (int)(IdKey & IdKey.KeyMask);

        internal byte TraitIndex => (byte)(IdKey & IdKey.IndexMask);
        internal byte TraitIndexOf(IdKey idKe) => (byte)(idKe & IdKey.IndexMask);
        internal bool IsErrorAux => (IdKey & IdKey.IsErrorAux) != 0;
        internal bool IsErrorAux1 => (IdKey & IdKey.IsErrorAux1) != 0;
        internal bool IsErrorAux2 => (IdKey & IdKey.IsErrorAux2) != 0;
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
        const byte B1 = 0x1;
        internal bool IsDeleted { get { return (_flags & B2) != 0; } set { _flags = value ? (byte)(_flags | B2) : (byte)(_flags & ~B2); } }
        const byte B2 = 0x2;
        internal bool AutoExpandLeft { get { return (_flags & B3) != 0; } set { _flags = value ? (byte)(_flags | B3) : (byte)(_flags & ~B3); } }
        const byte B3 = 0x4;
        internal bool AutoExpandRight { get { return (_flags & B4) != 0; } set { _flags = value ? (byte)(_flags | B4) : (byte)(_flags & ~B4); } }
        const byte B4 = 0x8;
        #endregion

        #region StringKeys  ===================================================
        internal string KindKey => GetKindKey(IdKey);
        internal string NameKey => GetNameKey(IdKey);
        internal string SummaryKey => GetSummaryKey(IdKey);
        internal string DescriptionKey => GetDescriptionKey(IdKey);

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
    }
}
