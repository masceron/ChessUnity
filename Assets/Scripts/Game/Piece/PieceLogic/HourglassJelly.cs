using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using System.Linq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJelly : Commons.PieceLogic, IPieceWithSkill
    {

        public HourglassJelly(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            // ActionManager.ExecuteImmediately(new ApplyEffect(new HourglassJellyEffect(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    var (rank, file) = BoardUtils.RankFileOf(Pos);

                    for (var x = rank - 4; x <= rank + 4; ++x)
                    {
                        for (var y = file - 4; y <= file + 4; ++y)
                        {
                            var piece = BoardUtils.PieceOn(BoardUtils.IndexOf(x, y));
                            if (piece == null || piece.Equals(this) || piece.PreviousMoves.Count <= 0) continue;
                            list.Add(new HourglassJellyActive(Pos, piece.Pos));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = BoardUtils.RankFileOf(Pos);
                        for (var x = rank - 4; x <= rank + 4; ++x)
                        {
                            for (var y = file - 4; y <= file + 4; ++y)
                            {
                                if (VerifyBounds(x) && VerifyBounds(y) && IsActive(IndexOf(x, y)) && IndexOf(x, y) != Pos)
                                {
                                    list.Add(new HourglassJellyActive(Pos, IndexOf(x, y)));
                                }
                            }
                        }
                        return;
                    }
                    var makerPiece = PieceOn(Pos);
                    if (makerPiece == null) return;

                    var (r, f) = RankFileOf(Pos);
                    var enemies = GetPiecesInRadius(r, f, 4, p => p != null && p.Color != makerPiece.Color);

                    int bestScore = enemies.Max(p => p.GetValueForAI());
                    var top = enemies.Where(p => p.GetValueForAI() == bestScore).ToList();
                    var chosen = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

                    list.Add(new HourglassJellyActive(Pos, chosen.Pos));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}