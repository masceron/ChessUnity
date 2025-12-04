using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class Phronima : Commons.PieceLogic, IPieceWithSkill
    {
        public Phronima(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
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
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}