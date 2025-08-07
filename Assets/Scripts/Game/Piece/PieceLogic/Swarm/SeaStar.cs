using System.Collections.Generic;
using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Moves;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Swarm
{
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

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}