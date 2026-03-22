using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class GoldenBassletActive : Action, ISkills
    {
        public GoldenBassletActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            var maker = PieceOn(Maker);
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(maker.GetStat(SkillStat.Duration), 100, PieceOn(Target)), maker)); // TODO: Check probability.
            SetCooldown(Maker, ((IPieceWithSkill)maker).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}