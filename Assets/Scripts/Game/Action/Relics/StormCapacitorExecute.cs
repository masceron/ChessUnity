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
        [MemoryPackInclude]
        private readonly int rank;
        [MemoryPackInclude]
        private readonly int file;
        [MemoryPackInclude]
        private readonly int size;
        [MemoryPackInclude]
        private readonly Corner corner;
        [MemoryPackInclude]
        private readonly bool Color;
        
        public StormCapacitorExecute(int rank, int file, int size, Corner corner, bool color) : base(-1)
        {
            this.rank = rank;
            this.file = file;
            this.size = size;
            this.corner = corner;
            Color = color;
        }

        protected override void ModifyGameState()
        {
            var pieces = BoardUtils.GetPiecesInSize(rank, file, size, corner, _ => true);
            
            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == Color) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, piece)));
            }
        }
    }
}
