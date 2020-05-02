
namespace ModelGraph.Core
{
    public abstract class ItemChange : Item
    {
        internal string _name;
        internal override string Name { get => _name; }

        #region Properties/Methods  ===========================================
        internal ChangeSet ChangeSet => Owner as ChangeSet;
        internal bool CanUndo => !IsUndone;
        internal bool CanRedo => IsUndone;

        protected void UpdateDelta()
        {
            Owner.ModelDelta++;
            Owner.Owner.ModelDelta++;
        }
        #endregion
    }
}
