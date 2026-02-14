using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.Augmentation
{
    public class PuppeteerSpiritPassive : Effect, IStartTurnTrigger
    {
        public PuppeteerSpiritPassive(PieceLogic piece) : base(-1, 1, piece, "effect_puppeteer_spirit_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAnyTurn;
        }


        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            Debug.Log(MatchManager.Ins.GameState.CurrentTurn);
            if (MatchManager.Ins.GameState.CurrentTurn == 1)
                foreach (var piece in BoardUtils.FindPiece<PieceLogic>(Piece.Color))
                    ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, piece)));
        }
    }
}