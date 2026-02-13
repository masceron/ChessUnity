using MemoryPack;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class StormCapacitorExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private StormCapacitorExecute() { }

        [MemoryPackInclude]
        private int _rank;
        [MemoryPackInclude]
        private int _file;
        [MemoryPackInclude]
        private int _size;
        [MemoryPackInclude]
        private Corner _corner;
        [MemoryPackInclude]
        private bool _color;
        
        public StormCapacitorExecute(int rank, int file, int size, Corner corner, bool color) : base(-1)
        {
            this._rank = rank;
            this._file = file;
            this._size = size;
            this._corner = corner;
            _color = color;
        }

        protected override void ModifyGameState()
        {
            var pieces = BoardUtils.GetPiecesInSize(_rank, _file, _size, _corner, _ => true);
            
            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == _color) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, piece)));
            }
        }
    }
}
