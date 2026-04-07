using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class ToxicZoanthid : Commons.PieceLogic, IPieceWithSkill
    {
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public ToxicZoanthid(PieceConfig cfg) : base(cfg, CoralMoves.Quiets, RookMoves.Captures)
        {
            SetStat(SkillStat.Target, 1);
            SetStat(SkillStat.Range, 2);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer){
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                    {
                        var index = IndexOf(rank, file);
                        if (!VerifyIndex(index) || !IsActive(index)) continue;

                        //Làm lại
                        //list.Add(new ToxicZoanthidPending(this, index));
                    }
                }
                
            };
        }
    }
}