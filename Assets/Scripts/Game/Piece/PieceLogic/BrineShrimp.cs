using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Skills;

namespace Game.Piece.PieceLogic
{
    public class BrineShrimp : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Target = 3;
        private const int Duration = 3;
        public BrineShrimp(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Extremophile(this)));

            SetStat(SkillStat.Target, Target);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    list.Add(new BrineShrimpActive(this, Duration));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}