using Game.Board.General;
using Game.Board.Interaction;
using UnityEngine;

namespace Game.Board.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalCapture: Action, ICaptures
    {
        public NormalCapture(ushort caller, ushort f, ushort t) : base(caller, true)
        {
            From = f;
            To = t;
        }
        public override void ApplyAction(GameState state)
        {
            if (Success == ActionResult.Succeed)
            {
                Object.Destroy(MatchManager.PieceManager.GetPiece(To).gameObject);
                InteractionManager.PieceManager.Move(From, To);
                ModifyGameState(state);
            }
            else
            {
                Debug.Log("Action failed");
            }
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(To);
            state.Move(From, To);
            Caller = To;
        }
    }
}