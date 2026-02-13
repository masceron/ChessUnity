using System.Collections.Generic;
using Game.Action.Captures;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
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
            foreach (var enemy in markedEnemies)
                if (enemy != null && IsAlive(enemy))
                {
                    var alreadyExists = actions.Any(a => a is ICaptures && a.Target == enemy.Pos);
                    if (!alreadyExists) actions.Add(new NormalCapture(caller.Pos, enemy.Pos));
                }
        }
    }
}