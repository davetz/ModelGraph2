
namespace ModelGraph.Core
{
    public abstract class ItemChange : Item
    {
        internal string _name;
        internal override string Name { get => _name; }
        internal override State State { get; set; }


        internal Change ChangeSet => Owner as Change;
        internal bool CanUndo => !IsUndone;
        internal bool CanRedo => IsUndone;

        abstract internal void Undo();
        abstract internal void Redo();

        protected void UpdateDelta()
        {
            Owner.ModelDelta++;
            Owner.Owner.ModelDelta++;
        }
    }
}
