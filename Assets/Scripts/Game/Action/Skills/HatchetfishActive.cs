using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using Game.AI;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class HatchetfishActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue => PieceOn(Target).Color != PieceOn(Maker).Color ? -15 : 0;

        public HatchetfishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target =  target;
        }

        protected override void ModifyGameState()
        {
            //Apply effect Marked no duration
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(-1, BoardUtils.PieceOn(Target))));

            var targetPiece = BoardUtils.PieceOn(Target);
            if (targetPiece == null) return;

            if (targetPiece.Effects.Any(e => e.EffectName == "effect_camouflage"))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece)));
            }

            BoardUtils.SetCooldown(Maker, ((IPieceWithSkill)BoardUtils.PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}
