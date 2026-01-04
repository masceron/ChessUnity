using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Traits;
using UnityEngine;
using Game.Piece;
using System.Linq;
using Game.Action.Captures;
namespace Game.Effects.Augmentation
{
    public class CrownOfSilencePassive : Effect, IApplyEffect
    {
        public CrownOfSilencePassive(PieceLogic piece) : base(-1, 1, piece, "effect_crown_of_silence_passive")
        {
        }

        private readonly List<string> blockedEffects = new()
        {
            "effect_purify",
        };
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;
            var effect = applyEffect.Effect;
            if (blockedEffects.Contains(effect.EffectName))
            {
                applyEffect.Result = ResultFlag.Blocked;
            }
        }
    }
}