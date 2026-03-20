using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class WeepingToadfish : Commons.PieceLogic, IPieceWithSkill
    {
        public WeepingToadfish(PieceConfig cfg) : base(cfg, PawnPushMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Ambush(this)));
            SetStat(SkillStat.Range, 4);
            SetStat(SkillStat.Duration, 2);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach(var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                    {
                        var pieceOn = PieceOn(IndexOf(rankOff, fileOff));
                        if (pieceOn != null && pieceOn.Color != this.Color)
                        {
                            list.Add(new WeepingToadfishActive(Pos, IndexOf(rankOff, fileOff), GetStat(SkillStat.Duration)));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}