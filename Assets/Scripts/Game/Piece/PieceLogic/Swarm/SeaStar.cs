using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Movesets;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Swarm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaStar: PieceLogic, IPieceWithSkill
    {
        public SeaStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0 ||
                    (!Color ? WhiteCaptured() : BlackCaptured()).All(p => p.Type != PieceType.SeaStar)) return;
                var (startRank, startFile) = RankFileOf(Pos);
                foreach (var (rank, file) in MoveEnumerators.Around(startRank, startFile, 1))
                {
                    var idx = IndexOf(rank, file);
                    if (PieceOn(idx) == null)
                    {
                        list.Add(new SeaStarResurrect(Pos, idx));
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}