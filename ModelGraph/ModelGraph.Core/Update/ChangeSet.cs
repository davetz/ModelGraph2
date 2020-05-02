using System;

namespace ModelGraph.Core
{
    public class ChangeSet : StoreOf<ItemChange>
    {
        internal DateTime DateTime;
        internal int Sequence;

        #region Constructor  ==================================================
        internal ChangeSet(StoreOf_ChangeSet owner, int seqno)
        {
            Owner = owner;
            OldIdKey = IdKey.ChangeSet;
            DateTime = DateTime.Now;
            Sequence = seqno;
            IsVirgin = true;
            SetCapacity(11);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal StoreOf_ChangeSet ChangeRoot => Owner as StoreOf_ChangeSet;
        internal bool CanUndo => (!IsCongealed && !IsUndone);
        internal bool CanRedo => (!IsCongealed && IsUndone);
        internal bool CanMerge => ChangeRoot.CanMerge(this); 
        internal void Merge() { ChangeRoot.Mege(this); }
        internal override string Name { get => Sequence.ToString(); }
        #endregion
    }
}
