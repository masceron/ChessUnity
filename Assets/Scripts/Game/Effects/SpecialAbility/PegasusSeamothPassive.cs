using System.Collections.Generic;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.SpecialAbility
{
    public class PegasusSeamothPassive : Effect, IOnMoveGenTrigger
    {
        public PegasusSeamothPassive(PieceLogic piece) : base(-1, 1, piece, "effect_pegasus_seamoth_passive")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;

            for (var i = 0; i < actions.Count; i++)
                if (actions[i] is IQuiets)
                    actions[i] = new PegasusSeamothMove(Piece.Pos, actions[i].GetTargetPos());
        }
    }
}