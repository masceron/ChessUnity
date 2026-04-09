using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaStar : Commons.PieceLogic, IPieceWithSkill
    {
        public SeaStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    if (SkillCooldown != 0 ||
                        GetCapturedOf(Color).All(p => p.Type != "piece_sea_star")) return;
                    var (startRank, startFile) = RankFileOf(Pos);
                    foreach (var (rank, file) in MoveEnumerators.Around(startRank, startFile, 1))
                    {
                        var idx = IndexOf(rank, file);
                        if (PieceOn(idx) == null) list.Add(new SeaStarResurrect(this, idx));
                    }
                }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}