using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilusActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -5 : 0;
        }

        public ChamberedNautilusActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            List<PieceLogic> bestPieces = new List<PieceLogic>();
            PieceLogic bestPiece = null;
            int maxPoint = int.MinValue;
            
            var (rank, file) = RankFileOf(Maker);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn.Pos == Maker || pOn.Color == PieceOn(Maker).Color
                    || pOn.Effects.Any(effect => effect.EffectName == "effect_bound" || effect.EffectName == "effect_extremophiles")) continue;
                
                int AIValue = pOn.GetValueForAI();
                if (AIValue > maxPoint)
                {
                    bestPieces.Clear();
                    bestPieces.Add(pOn);
                    maxPoint = AIValue;
                }
                else if (AIValue == maxPoint) bestPieces.Add(pOn);
            }

            if (bestPieces.Count == 0)
            {
                //
            }
            else if (bestPieces.Count == 1)
            {
                bestPiece = bestPieces[0];
            }
            else
            {
                bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
            }

            if (bestPiece != null)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, bestPiece)));
                SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            }
        }
    }
}