using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class RoyalGramma : Commons.PieceLogic, IPieceWithSkill
    {
        public RoyalGramma(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, ShellfishMoves.Captures)
        {
            SetStat(SkillStat.Target, 1);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 5, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Relentless(this, 1)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new RoyalGrammaPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    var allies = FindPiece<Commons.PieceLogic>(Color);
                    foreach(var ally in allies)
                    {
                        if (ally != this)
                        {
                            //Làm lại
                            //list.Add(new RoyalGrammaPending(this, ally));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}