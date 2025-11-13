using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class Phronima : PieceLogic, IPieceWithSkill
    {
        public Phronima(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                var (rank, file) = BoardUtils.RankFileOf(Pos);
                for (var x = rank - 3; x <= rank + 3; ++x)
                {
                    for (var y = file - 3; y <= file + 3; ++y)
                    {
                        if (!BoardUtils.VerifyBounds(x) || !BoardUtils.VerifyBounds(y)) continue;
                        var targetPiece = BoardUtils.PieceOn(BoardUtils.IndexOf(x, y));
                        if (targetPiece == null || targetPiece.Color == Color) continue;
                        list.Add(new Action.Skills.PhronimaActive(Pos, targetPiece.Pos));
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}