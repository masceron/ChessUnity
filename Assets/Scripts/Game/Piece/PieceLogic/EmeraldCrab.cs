using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using Game.Common;

namespace Game.Piece.PieceLogic
{
    public class EmeraldCrab : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 3;
        private const int Target = 1;
        private const int Duration = 3;

        public EmeraldCrab(PieceConfig cfg) : base(cfg, CrabMoves.Quiets, None.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));

            SetStat(SkillStat.Target, Target);
            SetStat(SkillStat.Range, Range);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, GetStat(SkillStat.Range));

                    foreach (var targetPiece in listPieces)
                    {
                        list.Add(new EmeraldCrabPending(this, targetPiece, Duration,
                            Mathf.Min(listPieces.Count, GetStat(SkillStat.Target))));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}