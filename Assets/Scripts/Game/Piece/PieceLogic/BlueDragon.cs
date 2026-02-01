using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Others;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class BlueDragon : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public BlueDragon(PieceConfig cfg) : base(cfg, SpinningMoves.Quiets, SpinningMoves.Captures)
        {
            // ActionManager.ExecuteImmediately(new ApplyEffect(new Silenced(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new BlueDragonPassive(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this) continue;
                        if (pOn.Color != Color)
                        {
                            list.Add(new BlueDragonActive(Pos, index));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        List<Commons.PieceLogic> bestPieces = new List<Commons.PieceLogic>();
                        Commons.PieceLogic bestPiece = null;
                        int maxStackPoison = int.MinValue;
            
                        var (rank, file) = RankFileOf(Pos);
            
                        // Find enemies with the highest stack of poison in a radius of 2.
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Pos == Pos || pOn.Color == Color) continue;
                
                            int stackPoison = pOn.Effects.Find(effect => effect.EffectName == "effect_poison")?.Strength ?? 0;
                            if (stackPoison > maxStackPoison && stackPoison > 0)
                            {
                                bestPieces.Clear();
                                bestPieces.Add(pOn);
                                maxStackPoison = stackPoison;
                            }
                            else if (stackPoison == maxStackPoison) bestPieces.Add(pOn);
                        }
                    
                        if (bestPieces.Count == 1)
                        {
                            bestPiece = bestPieces[0];
                        }
                        else if (bestPieces.Count > 1)
                        {
                            bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
                        }
                        else if (bestPieces.Count == 0)
                        {
                            // Find enemies which have the highest AI value and don't have extremophiles in a radius of 2.
                            int maxAIValue = int.MinValue;
                            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                            {
                                var index = IndexOf(rankOff, fileOff);
                                var pOn = PieceOn(index);
                                if (pOn == null || pOn.Pos == Pos || pOn.Color == Color
                                    || pOn.Effects.Any(effect => effect.EffectName == "effect_extremophiles")) continue;
                
                                int aiValue = pOn.GetValueForAI();
                                if (aiValue > maxAIValue)
                                {
                                    bestPieces.Clear();
                                    bestPieces.Add(pOn);
                                    maxAIValue = aiValue;
                                }
                                else if (aiValue == maxAIValue) bestPieces.Add(pOn);
                            }

                            if (bestPieces.Count == 1)
                            {
                                bestPiece = bestPieces[0];
                            }
                            else if (bestPieces.Count > 1)
                            {
                                bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
                            }
                            else if(bestPieces.Count == 0)
                            {
                                //
                            }
                        }
                        
                        if (bestPiece != null)
                        {
                            list.Add(new BlueDragonActive(Pos, bestPiece.Pos));
                        }
                        
                    }
                    else
                    {
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            list.Add(new BlueDragonActive(Pos, index));
                        }
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}