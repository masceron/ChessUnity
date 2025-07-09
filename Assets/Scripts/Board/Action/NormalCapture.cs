using Board.Interaction;
using Core;
using UnityEngine;

namespace Board.Action
{
    public class NormalCapture: Action
    {
        public NormalCapture(int caller, ushort f, ushort t) : base(caller)
        {
            From = f;
            To = t;
        }
        public override void ApplyAction(GameState state)
        {
            Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
            InteractionManager.PieceManager.Move(From, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var pieceAffected = state.MainBoard[From];
            var pieceToMove = state.MainBoard[From];
            state.RemoveTrigger(pieceAffected);
            state.MainBoard[To] = pieceToMove;
            state.MainBoard[To].Pos = To;
            state.MainBoard[From] = null;
            state.LastMove = this;
            state.LastMovedPiece = pieceToMove;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}