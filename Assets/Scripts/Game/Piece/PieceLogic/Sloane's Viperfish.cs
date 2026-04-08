using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class SloaneSViperfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 4;
        public SloaneSViperfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var caller = PieceOn(Pos);

                    for (var i = -Range; i <= Range; i++)
                    {
                        if (!VerifyBounds(rank + i)) continue;
                        for (var j = -Range; j <= Range; j++)
                        {
                            if (!VerifyBounds(file + j)) continue;

                            var idx = IndexOf(rank + i, file + j);

                            var p = PieceOn(idx);
                            if (p == null || p == caller || p.Color == caller.Color) continue;

                            list.Add(new SloaneViperfishActive(this, p));
                        }
                    }
                }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; }
    }
}