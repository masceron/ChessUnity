// using Game.Action;
// using Game.Action.Internal;
// using Game.Piece.PieceLogic;
// using Game.Managers;
// using System.Collections.Generic;

// namespace Game.Effects.Traits
// {
//     public class HourglassJellyEffect: Effect, IApplyEffect
//     {
//         public HourglassJellyEffect(PieceLogic piece) : base(-1, 1, piece, EffectName.HourglassJelly)
//         {
//             MatchManager.Ins.GameState.AddObserver(this);
//             this.ObserverActivateWhen = ObserverActivateWhen.Moves;
//         }

//         public void OnCallApplyEffect(ApplyEffect applyEffect)
//         {
//             if (applyEffect.Effect.Piece != Piece) return;

//             var effect = applyEffect.Effect;

//             if (effect.EffectName is EffectName.Slow or EffectName.Haste)
//             {
//                 applyEffect.Result = ActionResult.Failed;
//             }
//         }
//         public override void OnCallMoveGen(List<Action.Action> actions)
//         {
            
//         }
//     }
// }