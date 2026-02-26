using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Others;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class ScalyheadSculpin : Commons.PieceLogic, IPieceWithSkill
    {
        public ScalyheadSculpin(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ScalyheadSculpinPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    list.Add(new ScalyheadSculpinActive(Pos));
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; }
    }
}