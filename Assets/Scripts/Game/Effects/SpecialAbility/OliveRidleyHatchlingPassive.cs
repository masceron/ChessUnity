using Game.Action.Skills;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OliveRidleyHatchlingPassive : Effect, IStartTurnTrigger
    {
        private readonly int CounterTurn;
        private int currentTurn;

        public OliveRidleyHatchlingPassive(PieceLogic piece, int counterTurn) : base(-1, 1, piece, "effect_olive_ridley_hatchling_passive")
        {
            CounterTurn = counterTurn;
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Other;

        StartTurnEffectType IStartTurnTrigger.StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;

        void IStartTurnTrigger.OnCallStart(Action.Action lastMainAction)
        {
            currentTurn++;
            if (currentTurn >= CounterTurn)
            {
                currentTurn = 0;
                if (Piece is OliveRidleyHatchling oliveRidleyHatchling)
                    oliveRidleyHatchling.SetSkillHandler(Skill);

                Piece.SetHasSkill(true);
            }
        }

        void Skill(List<Action.Action> list, bool isPlayer, bool excludeEmptyTile)
        {
            if (isPlayer)
            {
                list.Add(new OliveRidleyHatchlingActive(Piece.Pos));
            }
        }
    }
}