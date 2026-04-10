using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ChrysosUpgrade : Action, ISkills
    {
        [MemoryPackInclude] private byte _cost;

        [MemoryPackInclude] private PieceConfig _swapTo;

        [MemoryPackConstructor]
        private ChrysosUpgrade()
        {
        }

        public ChrysosUpgrade(PieceLogic maker, PieceLogic target, PieceConfig swap, byte cost) : base(maker, target)
        {
            _cost = cost;
            _swapTo = swap;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = GetMakerAsPiece();
            ActionManager.EnqueueAction(new DestroyPiece(GetTargetAsPiece()));
            ActionManager.EnqueueAction(new SpawnPiece(_swapTo));
            ((Chrysos)pieceOn).Coin -= _cost;
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}