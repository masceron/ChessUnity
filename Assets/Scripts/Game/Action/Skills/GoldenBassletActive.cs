using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class GoldenBassletActive : Action, ISkills
    {
        public GoldenBassletActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            var maker = GetMaker() as PieceLogic;
            ActionManager.EnqueueAction(
                new ApplyEffect(new Blinded(maker.GetStat(SkillStat.Duration), 100, GetTarget() as PieceLogic),
                    maker)); // TODO: Check probability.
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)maker).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}