using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using MemoryPack;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class StormCapacitorExecute : Action, IRelicAction
    {
        [MemoryPackInclude] private bool _color;

        [MemoryPackInclude] private Corner _corner;

        [MemoryPackInclude] private int _file;

        [MemoryPackInclude] private int _rank;

        [MemoryPackInclude] private int _size;

        [MemoryPackConstructor]
        private StormCapacitorExecute()
        {
        }

        public StormCapacitorExecute(int rank, int file, int size, Corner corner, bool color) : base(null)
        {
            _rank = rank;
            _file = file;
            _size = size;
            _corner = corner;
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