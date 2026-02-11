
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.AI;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public class SloaneSViperfishActive : Action, ISkills, IAIAction
    {
        private readonly bool bleeding;
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        public SloaneSViperfishActive(int maker, bool _bleeding) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
            bleeding = _bleeding;
        }

        protected override void ModifyGameState()
        {
            if (bleeding)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(Target)), PieceOn(Maker)));
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, PieceOn(Target)), PieceOn(Maker)));
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}