using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class StalkerInstinctEffect : Effect, IOnMoveGenTrigger
    {
        public StalkerInstinctEffect(PieceLogic piece) : base(-1, 1, piece, "effect_stalker_instinct")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            var markedEnemies = FindPiecesWithEffectName(!caller.Color, "effect_marked");
            foreach (PieceLogic enemy in from enemy in markedEnemies
                     where enemy != null && IsAlive(enemy)
                     let alreadyExists = actions.Any(a => a is ICaptures && a.GetTarget() == enemy)
                     where !alreadyExists
                     select enemy) actions.Add(new NormalCapture(caller, enemy));
        }
    }
}