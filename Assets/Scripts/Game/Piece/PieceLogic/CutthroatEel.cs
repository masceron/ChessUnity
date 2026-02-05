using Game.Movesets;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects;
using ZLinq;

namespace Game.Piece.PieceLogic
{
    public class CutthroatEel : Commons.PieceLogic, IPieceWithSkill
    {
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public CutthroatEel(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Pos), FileOf(Pos), 4))
                    {
                        int targetPos = IndexOf(rank, file);
                        Commons.PieceLogic pieceOn = PieceOn(targetPos);

                        if (pieceOn != null && pieceOn.Color != Color && pieceOn.Effects.Any(e => e.EffectName == "effect_bleeding"))
                        {
                            foreach (Effect effect in pieceOn.Effects)
                            {
                                if (effect is Bleeding bleeding)
                                {
                                    list.Add(new CutthroatEelActive(Pos, targetPos, bleeding));
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var (rank, file) = RankFileOf(Pos);
                    var enemiesWithBleeding = GetPiecesInRadius(rank, file, 4, p => p != null && p.Color != Color);
                    
                    // Filter kẻ địch với Bleeding effect
                    var validTargets = new System.Collections.Generic.List<(Commons.PieceLogic piece, Bleeding bleeding)>();
                    foreach (var enemy in enemiesWithBleeding)
                    {
                        foreach (Effect effect in enemy.Effects)
                        {
                            if (effect is Bleeding bleeding)
                            {
                                validTargets.Add((enemy, bleeding));
                                break;
                            }
                        }
                    }
                    
                    if (validTargets.Count == 0) return;
                    
                    // List A: Bleeding <= 3
                    var listA = validTargets.Where(t => t.bleeding.Strength <= 3).ToList();
                    
                    System.Collections.Generic.List<(Commons.PieceLogic piece, Bleeding bleeding)> candidates;
                    
                    if (listA.Count > 0)
                    {
                        // Tìm candidate có value cao nhất
                        int maxValue = listA.Max(t => t.piece.GetValueForAI());
                        candidates = listA.Where(t => t.piece.GetValueForAI() == maxValue).ToList();
                    }
                    else
                    {
                        // List B: Bleeding >= 3
                        var listB = validTargets.Where(t => t.bleeding.Strength >= 3).ToList();
                        if (listB.Count == 0) return;
                        
                        // Tìm candidate có value cao nhất
                        int maxValue = listB.Max(t => t.piece.GetValueForAI());
                        candidates = listB.Where(t => t.piece.GetValueForAI() == maxValue).ToList();
                    }
                    
                    // Chọn 1 hoặc random
                    var chosen = candidates.Count == 1 ? candidates[0] : candidates[UnityEngine.Random.Range(0, candidates.Count)];
                    list.Add(new CutthroatEelActive(Pos, chosen.piece.Pos, chosen.bleeding));
                }
            };
        }
    }
}