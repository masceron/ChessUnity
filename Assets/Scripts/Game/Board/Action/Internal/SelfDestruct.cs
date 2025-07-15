using Game.Board.General;
using UnityEngine;

namespace Game.Board.Action.Internal
{
    public class SelfDestruct: Action, IInternal
    {
        public SelfDestruct(int caller) : base(caller, false)
        {}

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(MatchManager.PieceManager.GetPiece(Caller).gameObject);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(Caller);
        }
    }
}