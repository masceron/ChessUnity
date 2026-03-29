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

        public ChrysosUpgrade(int maker, PieceConfig target, byte cost) : base(maker)
        {
            _target = target;
            _cost = cost;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = GetMaker() as PieceLogic;
            ActionManager.EnqueueAction(new DestroyPiece(_target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(_target));
            ((Chrysos)pieceOn).Coin -= _cost;
            SetCooldown(pieceOn, ((IPieceWithSkill)pieceOn).TimeToCooldown);
        }
    }
}