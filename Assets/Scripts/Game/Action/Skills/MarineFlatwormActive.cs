using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MarineFlatwormActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MarineFlatwormActive()
        {
        }

        public MarineFlatwormActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        protected override void ModifyGameState()
        {
            var config = new PieceConfig(GetMakerAsPiece().Type, GetMakerAsPiece().Color, GetTargetPos());
            ActionManager.EnqueueAction(new SpawnPieceWithEffect(config, new Illusion(GetTargetAsPiece())));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}