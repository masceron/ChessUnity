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
    public class WhiteMonkfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Target = 1;
        private const int Range = 4;
        private const int Duration = 1;
        public WhiteMonkfish(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Ambush(this)));
            SetStat(SkillStat.Target, Target);
            SetStat(SkillStat.Range, Range);
            SetStat(SkillStat.Duration, Duration);

            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, GetStat(SkillStat.Range));
                    foreach (var targetPiece in listPieces)
                    {
                        list.Add(new WhiteMonkfishActive(this, targetPiece, GetStat(SkillStat.Duration)));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}