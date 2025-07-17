using Game.Board.General;
using Game.Board.Interaction;
using UnityEngine;

namespace Game.Board.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalCapture: Action, ICaptures
    {
        public NormalCapture(ushort caller, int f, int t) : base(caller, true)
        {
            From = (ushort)f;
            To = (ushort)t;
        }
        public override void ApplyAction(GameState state)
        {
            if (Success == ActionResult.Succeed)
            {
                MatchManager.PieceManager.Destroy(To);
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