using System;

namespace ModelGraph.Core
{
    public class Change : StoreOf<ItemChange>
    {
        internal DateTime DateTime;
        internal int Sequence;
        internal override IdKey IdKey => IdKey.ChangeSet;

        #region Constructor  ==================================================
        internal Change(ChangeRoot owner, int seqno)
        {
            Owner = owner;
            DateTime = DateTime.Now;
            Sequence = seqno;
            IsVirgin = true;
            SetCapacity(11);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal ChangeRoot ChangeRoot => Owner as ChangeRoot;
        internal bool CanUndo => (!IsCongealed && !IsUndone);
        internal bool CanRedo => (!IsCongealed && IsUndone);
        internal bool CanMerge => ChangeRoot.CanMerge(this); 
        internal void Merge() { ChangeRoot.Mege(this); }
        internal override string Name { get => Sequence.ToString(); }

        internal void Undo()
        {
            foreach (var item in Items)
            {
                item.Undo();
            }
            IsUndone = true;
        }

        internal void Redo()
        {
            foreach (var item in Items)
            {
                item.Redo();
            }
            IsUndone = false;
        }
        #endregion
    }
}
