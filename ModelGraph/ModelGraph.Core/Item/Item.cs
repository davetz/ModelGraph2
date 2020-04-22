using System;

namespace ModelGraph.Core
{
    public class Item
    {
        internal Item Owner;        //each item has an owner, this-> owner-> ... -> dataChef

        internal IdKey Trait;       //identity, static flags, and resource string key
        private State _state;       //bit flags specific to each item type

        private byte _flags;        //IsNew, IsDeleted, AutoExpandLeft, AutoExpandRight,..
        internal byte ModelDelta;   //incremented when a property or relation is changed
        internal byte ChildDelta;   //incremented when list of child items is changed 
        internal byte ErrorDelta;   //incremented when item's error state has changed

        #region Trait  ========================================================
        //internal bool IsExternal => (Trait & Trait.IsExternal) != 0;
        //internal bool IsInternal => (Trait & Trait.IsInternal) != 0;

        internal bool IsDataChef => (Trait == IdKey.DataChef);
        internal bool IsViewX => (Trait == IdKey.ViewX);
        internal bool IsPairX => (Trait == IdKey.PairX);
        internal bool IsRowX => (Trait == IdKey.RowX);
        internal bool IsEnumX => (Trait == IdKey.EnumX);
        internal bool IsTableX => (Trait == IdKey.TableX);
        internal bool IsGraphX => (Trait == IdKey.GraphX);
        internal bool IsQueryX => (Trait == IdKey.QueryX);
        internal bool IsSymbolX => (Trait == IdKey.SymbolX);
        internal bool IsColumnX => (Trait == IdKey.ColumnX);
        internal bool IsComputeX => (Trait == IdKey.ComputeX);
        //internal bool IsCommandX => (Trait == Trait.CommandX);
        internal bool IsRelationX => (Trait == IdKey.RelationX);
        internal bool IsGraph => (Trait == IdKey.Graph);
        internal bool IsNode => (Trait == IdKey.Node);
        internal bool IsEdge => (Trait == IdKey.Edge);

        internal bool IsItemMoved => Trait == IdKey.ItemMoved;
        internal bool IsItemCreated => Trait == IdKey.ItemCreated;
        internal bool IsItemUpdated => Trait == IdKey.ItemUpdated;
        internal bool IsItemRemoved => Trait == IdKey.ItemRemoved;
        internal bool IsItemLinked => Trait == IdKey.ItemLinked;
        internal bool IsItemUnlinked => Trait == IdKey.ItemUnlinked;
        internal bool IsItemLinkMoved => Trait == IdKey.ItemChildMoved;


        internal bool IsExternal => (Trait & IdKey.IsExternal) != 0;
        internal bool IsReference => (Trait & IdKey.IsReference) != 0;
        internal bool IsCovert => (Trait & IdKey.SubMask) == IdKey.IsCovert;
        internal bool IsReadOnly => (Trait & IdKey.SubMask) == IdKey.IsReadOnly;
        internal bool CanMultiline => (Trait & IdKey.SubMask) == IdKey.CanMultiline;

        internal int ItemKey => (int)(Trait & IdKey.KeyMask);

        internal byte TraitIndex => (byte)(Trait & IdKey.IndexMask);
        internal byte TraitIndexOf(IdKey trait) => (byte)(trait & IdKey.IndexMask);
        internal bool IsErrorAux => (Trait & IdKey.IsErrorAux) != 0;
        internal bool IsErrorAux1 => (Trait & IdKey.IsErrorAux1) != 0;
        internal bool IsErrorAux2 => (Trait & IdKey.IsErrorAux2) != 0;
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

        internal bool IsValueXHead => IsHead && QueryKind == QueryType.Value;// Trait == Trait.QueryXValueHead;

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
        internal string KindKey => GetKindKey(Trait);
        internal string NameKey => GetNameKey(Trait);
        internal string SummaryKey => GetSummaryKey(Trait);
        internal string DescriptionKey => GetDescriptionKey(Trait);

        internal string GetKindKey(IdKey trait) => $"{(int)(trait & IdKey.KeyMask):X3}K";
        internal string GetNameKey(IdKey trait) => $"{(int)(trait & IdKey.KeyMask):X3}N";
        internal string GetSummaryKey(IdKey trait) => $"{(int)(trait & IdKey.KeyMask):X3}S";
        internal string GetDescriptionKey(IdKey trait) => $"{(int)(trait & IdKey.KeyMask):X3}V";
        internal string GetAcceleratorKey(IdKey trait) => $"{(int)(trait & IdKey.KeyMask):X3}A".ToUpper();
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
