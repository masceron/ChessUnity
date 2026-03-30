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

        [MemoryPackInclude] private PieceConfig _target;

        [MemoryPackConstructor]
        private ChrysosUpgrade()
        {
        }

        public ChrysosUpgrade(PieceLogic maker, PieceLogic target, PieceConfig swap, byte cost) : base(maker, target)
        {
            _cost = cost;
            _target = swap;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = GetMaker() as PieceLogic;
            ActionManager.EnqueueAction(new DestroyPiece(GetTarget() as PieceLogic));
            ActionManager.EnqueueAction(new SpawnPiece(_target));
            ((Chrysos)pieceOn).Coin -= _cost;
            SetCooldown(pieceOn, ((IPieceWithSkill)pieceOn).TimeToCooldown);
        }
    }
}