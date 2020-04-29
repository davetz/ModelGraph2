
namespace ModelGraph.Core
{
    public class PairZ : Item
    {
        internal override IdKey ViKey => _idKey;
        private IdKey _idKey;

        internal PairZ(EnumZ owner, IdKey idKe)
        {
            Owner = owner;
            _idKey = idKe;

            owner.Add(this);
        }

        internal int EnumKey => (int)(_idKey & IdKey.EnumMask);
    }
}
