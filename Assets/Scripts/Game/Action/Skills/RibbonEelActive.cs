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
        [MemoryPackConstructor]
        private RibbonEelActive() { }

        [MemoryPackInclude]
        private int _sourcePiecePos;
        [MemoryPackInclude]
        private int _targetPiecePos;
        private const int BoundDuration = 1;
        
        public RibbonEelActive(int maker, int sourcePiece, int targetPiece) : base(maker)
        {
            Maker = maker;
            Target = maker;
            _sourcePiecePos = sourcePiece;
            _targetPiecePos = targetPiece;
        }

        protected override void ModifyGameState()
        {
            var sourcePiece = PieceOn(_sourcePiecePos);
            var targetPiece = PieceOn(_targetPiecePos);
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, sourcePiece), sourcePiece));
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(BoundDuration, targetPiece), sourcePiece));
            ActionManager.EnqueueAction(new NormalMove(_sourcePiecePos, Maker));
            
        }
    }
}