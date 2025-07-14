using Game.Board.General;
using UnityEngine;

namespace Game.Board.Action.Captures
{
    public class DestroyConstruct: Action, ICaptures
    {
        public DestroyConstruct(int caller, int to) : base(caller, false)
        {
            To = (ushort)to;
        }

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(MatchManager.PieceManager.GetPiece(Caller).gameObject);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(Caller);
            state.Destroy(To);
            state.ActiveBoard[To] = true;
        }
    }
}