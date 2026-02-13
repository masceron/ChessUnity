using System.Collections.Generic;
using Game.Action.Captures;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.SpecialAbility
{
    public class SeaUrchinPassive: Effect, IOnMoveGenTrigger
    {
        public SeaUrchinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_sea_urchin_passive")
        {}

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            for (var i = 0; i < actions.Count; i++)
            {
                if (actions[i] is ICaptures)
                    actions[i] = new DestroyConstruct(Piece.Pos, actions[i].Target);
            }
        }
    }
}