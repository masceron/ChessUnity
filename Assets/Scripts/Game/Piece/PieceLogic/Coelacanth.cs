using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class Coelacanth : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Target = 1;
        private const int Range = 3;
        private const int Duration = 2;
        public Coelacanth(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new FreeMovement(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Regenerative(this)));
            SetStat(SkillStat.Target, Target);
            SetStat(SkillStat.Range, Range);
            SetStat(SkillStat.Duration, Duration);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var piecesInRange = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, GetStat(SkillStat.Range));
                    foreach (var targetPiece in piecesInRange) { 
                        if (targetPiece.Effects.Any(e => e.EffectName == "effect_slow")) continue;
                        list.Add(new CoelacanthActive(this, targetPiece, GetStat(SkillStat.Duration)));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}