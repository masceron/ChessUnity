using Game.Board.General;
using UnityEngine;

namespace Game.Board.Action.Internal
{
    public class CarapaceKill: Action, IInternal
    {
        public CarapaceKill(int caller, int to) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        public override void ApplyAction(GameState state)
        {
            MatchManager.PieceManager.Destroy(To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(To);
        }
    }
}