using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class RibbonEelActive : Action
    {
        private const int BoundDuration = 1;

        [MemoryPackInclude] private int _sourcePiecePos;

        [MemoryPackInclude] private int _targetPiecePos;

        [MemoryPackConstructor]
        private RibbonEelActive()
        {
        }

        public RibbonEelActive(int maker, int sourcePiece, int targetPiece) : base(maker)
        {
            _sourcePiecePos = sourcePiece;
            _targetPiecePos = targetPiece;
        }

        protected override void ModifyGameState()
        {
            var sourcePiece = PieceOn(_sourcePiecePos);
            var targetPiece = PieceOn(_targetPiecePos);
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, sourcePiece), sourcePiece));
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, targetPiece), sourcePiece));
            ActionManager.EnqueueAction(new NormalMove(_sourcePiecePos, GetFrom()));
        }
    }
}