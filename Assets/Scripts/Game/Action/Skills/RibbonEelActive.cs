using MemoryPack;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class RibbonEelActive : Action
    {
        [MemoryPackInclude]
        private readonly int sourcePiecePos;
        [MemoryPackInclude]
        private readonly int targetPiecePos;
        private const int BoundDuration = 1;
        
        public RibbonEelActive(int maker, int sourcePiece, int targetPiece) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
            sourcePiecePos = sourcePiece;
            targetPiecePos = targetPiece;
        }

        protected override void ModifyGameState()
        {
            var sourcePiece = PieceOn(sourcePiecePos);
            var targetPiece = PieceOn(targetPiecePos);
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, sourcePiece), sourcePiece));
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, targetPiece), sourcePiece));
            ActionManager.EnqueueAction(new NormalMove(sourcePiecePos, Maker));
            
        }
    }
}