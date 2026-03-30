using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class AnglerfishTaunt : Action, ISkills
    {
        [MemoryPackConstructor]
        private AnglerfishTaunt()
        {
        }

        public AnglerfishTaunt(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker() as PieceLogic;
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -10;
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(3, GetTarget() as PieceLogic), GetMaker() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}