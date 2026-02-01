using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StormCapacitorExcute : Action, IRelicAction
    {
        private int rank;
        private int file;
        private int size;
        private Corner corner;
        private bool Color;
        
        public StormCapacitorExcute(int rank, int file, int size, Corner corner, bool color) : base(-1)
        {
            this.rank = rank;
            this.file = file;
            this.size = size;
            this.corner = corner;
            this.Color = color;
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
