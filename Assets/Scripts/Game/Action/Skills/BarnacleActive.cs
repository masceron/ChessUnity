using System.Linq;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.AI;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BarnacleActive: Action, ISkills, IAIAction
    {
        public BarnacleActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void Animate()
        {

        }

        protected override void ModifyGameState()
        {
            foreach (var effect in PieceOn(Target).Effects
                         .Where(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield"))
            {
                if (effect.Duration > 0)
                    effect.Duration -= 1;
                else
                {
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
                }
            }
            
            SetCooldown(Maker, -1);
            //SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var allPieces = MatchManager.Ins.GameState.PieceBoard;
            var listPieces = allPieces.Where(p => p != null && p.Color != PieceOn(Maker).Color &&
                                                 p.Effects.Any(e => e.EffectName == "effect_shield" || e.EffectName == "effect_hardened_shield")).ToList();

            if (listPieces.Count == 0) return;
            int maxValue = listPieces.Max(p => p.GetValueForAI());
            var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
            if (bestPieces.Count == 0) return;
            var random = new System.Random();
            var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
            
            foreach (var effect in selectedPiece.Effects
                         .Where(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield"))
            {
                if (effect.Duration > 0)
                    effect.Duration -= 1;
                else
                {
                    //ActionManager.EnqueueAction(new RemoveEffect(effect));
                    BoardViewer.Ins.ExecuteAction(new RemoveEffect(effect));
                }
            }
            SetCooldown(Maker, -1);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}