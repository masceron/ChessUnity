using Game.Effects;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class Phronima : Commons.PieceLogic, IPieceWithSkill
    {
        public Phronima(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);
                if (isPlayer)
                {
                    for (var x = rank - 3; x <= rank + 3; ++x)
                    {
                        for (var y = file - 3; y <= file + 3; ++y)
                        {
                            if (!VerifyBounds(x) || !VerifyBounds(y)) continue;
                            var targetPiece = PieceOn(IndexOf(x, y));
                            if (targetPiece == null || targetPiece.Color == Color) continue;
                            list.Add(new Action.Skills.PhronimaActive(Pos, targetPiece.Pos));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        var listPieces = GetPiecesInRadius(rank, file, 3, p => p != null && p.Color != Color);
                        if (listPieces.Count == 0) return;

                        listPieces.Sort((a, b) =>
                        {
                            int buffCountA = 0, buffCountB = 0;
                            foreach (var effect in a.Effects)
                            {
                                if (effect.Category == EffectCategory.Buff)
                                    buffCountA++;
                            }

                            foreach (var effect in b.Effects)
                            {
                                if (effect.Category == EffectCategory.Buff)
                                    buffCountB++;
                            }

                            return buffCountA.CompareTo(buffCountB);
                        });

                        var idx = Random.Range(0, listPieces.Count - 1);

                        list.Add(new Action.Skills.PhronimaActive(Pos, listPieces[idx].Pos));
                    }
                    else
                    {
                        for (var x = rank - 3; x <= rank + 3; x++)
                        {
                            for (var y = file - 3; y <= file + 3; y++)
                            {
                                if (x == rank && y == file) continue;
                                list.Add(new Action.Skills.PhronimaActive(Pos, IndexOf(x, y)));
                            }
                        }
                    }

                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}