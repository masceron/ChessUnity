using Game.Action.Internal;
using Game.AI;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArchelonShield: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -15;
            return 0;
        }

        public ArchelonShield(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}