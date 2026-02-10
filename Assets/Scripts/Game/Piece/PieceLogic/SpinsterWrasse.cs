using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Effects;
using Game.Effects.Buffs;
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
                if (isPlayer)
                {
                    foreach (var (nRank, nFile) in MoveEnumerators.AroundUntil(rank, file, 5))
                    {
                        var idx = IndexOf(nRank, nFile);
                        var pOn = PieceOn(idx);
                        if (pOn == null) continue;

                        list.Add(new SpinsterWrassePending(Pos, idx, Color));
                    }
                }
                else
                {
                    var listA = GetPiecesInRadius(rank, file, 5, p => p != null && p.Color == Color);
            
                    if (listA.Count == 0) return;
                    listA.Sort((a, b) =>
                    {
                        int buffCountA = 0, buffCountB = 0;
                        foreach (var effect in a.Effects)
                        {
                            if (effect.Category == EffectCategory.Debuff)
                                buffCountA++;
                        }

                        foreach (var effect in b.Effects)
                        {
                            if (effect.Category == EffectCategory.Debuff)
                                buffCountB++;
                        }

                        return buffCountA.CompareTo(buffCountB);
                    });

                    var listB = new List<Commons.PieceLogic>();
                    for (int i = 0; i < BoardSize; ++i)
                    {
                        var piece = PieceOn(i);
                        if (piece == null || piece.Color != Color || piece.Effects.Any(
                                e => e.EffectName == "effect_extremophiles" || e.EffectName == "effect_Adaptation"))
                            continue;
                
                        listB.Add(piece);
                    }
            
                    if (listB.Count == 0) return;
            
                    listB.Sort((a, b) => b.GetValueForAI().CompareTo(a.GetValueForAI()));
            
                    var idxA = UnityEngine.Random.Range(0, listA.Count - 1);
                    var idxB = UnityEngine.Random.Range(0, listB.Count - 1);

                    list.Add(new SpinsterWrasseBuff(Pos, listA[idxA].Pos, listB[idxB].Pos));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
}