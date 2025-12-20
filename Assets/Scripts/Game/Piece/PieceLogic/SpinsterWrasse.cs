using Game.Action.Skills;
using Game.Common;
using Game.Movesets;

using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class SpinsterWrasse : Commons.PieceLogic, IPieceWithSkill
    {
        public SpinsterWrasse(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, None.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);
                foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 5))
                {
                    var idx = IndexOf(nRank, nFile);
                    var pOn = PieceOn(idx);
                    if (pOn == null) continue;
                    
                    list.Add(new SpinsterWrasseActive(Pos, idx, Color));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
}