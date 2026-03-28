using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    public class Construct : Effect, IOnMoveGenTrigger
    {
        public Construct(PieceLogic piece) : base(-1, 1, piece, "effect_construct")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            if (actions == null || actions.Count == 0) return;

            for (var i = actions.Count - 1; i >= 0; i--)
                if (actions[i] is not DestroyConstruct && actions[i].GetTarget() == Piece)
                    actions.RemoveAt(i);
        }
    }
}