namespace ModelGraph.Core
{
    public abstract class Error : Item
    {
        internal Item Item;
        internal override string Name { get => _name; set => _name = value; }
        internal string _name;

        internal abstract void Add(string text);
        internal abstract void Clear();
        internal abstract int Count { get; }
        internal abstract string GetError(int index = 0);
    }
}
