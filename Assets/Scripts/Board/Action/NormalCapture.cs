using Board.Interaction;
using Core.General;
using UnityEngine;

namespace Board.Action
{
    public class NormalCapture: Action
    {
        public NormalCapture(ushort caller, ushort f, ushort t) : base(caller, true, true)
        {
            From = f;
            To = t;
        }
        public override void ApplyAction(GameState state)
        {
            if (Success)
            {
                Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
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
            state.LastMove = this;
            Caller = To;
        }
    }
}