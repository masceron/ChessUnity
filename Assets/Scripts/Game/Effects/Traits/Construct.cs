using Game.Piece.PieceLogic.Commons;
using Game.Action.Captures;
using System.Collections.Generic;

namespace Game.Effects.Traits
{
    public class Construct : Effect
    {
        public Construct(PieceLogic piece) : base(-1, 1, piece, "effect_construct")
        {
        }

        public override void OnCallMoveGen(List<Action.Action> actions)
        {
            if (actions == null || actions.Count == 0) return;

            for (var i = actions.Count - 1; i >= 0; i--)
            {
                if (!(actions[i] is DestroyConstruct) && actions[i].Target == Piece.Pos)
                {
                    actions.RemoveAt(i);
                }
            }
        }
    }
}