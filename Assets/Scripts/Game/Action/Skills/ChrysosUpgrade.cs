using MemoryPack;
using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ChrysosUpgrade : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        [MemoryPackInclude] private readonly PieceConfig _target;

        [MemoryPackInclude] private readonly byte _cost;

        public ChrysosUpgrade(int maker, PieceConfig target, byte cost) : base(maker)
        {
            _target = target;
            _cost = cost;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = PieceOn(Maker);
            ActionManager.EnqueueAction(new DestroyPiece(_target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(_target));
            ((Chrysos)pieceOn).Coin -= _cost;
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}