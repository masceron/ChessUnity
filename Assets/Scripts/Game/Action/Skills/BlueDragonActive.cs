using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class BlueDragonActive : Action, ISkills, IAIAction
    {
        public BlueDragonActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Poison(1, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        public void CompleteActionForAI()
        {
            List<PieceLogic> bestPieces = new List<PieceLogic>();
            PieceLogic bestPiece = null;
            int maxStackPoison = int.MinValue;
            
            var (rank, file) = RankFileOf(Maker);
            
            // Find enemies with the highest stack of poison in a radius of 2.
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn.Pos == Maker || pOn.Color == PieceOn(Maker).Color) continue;
                
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
                    if (pOn == null || pOn.Pos == Maker || pOn.Color == PieceOn(Maker).Color
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
                ActionManager.ExecuteImmediately(new ApplyEffect(new Poison(1, bestPiece)));
                SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            }
            
        }
    }
}