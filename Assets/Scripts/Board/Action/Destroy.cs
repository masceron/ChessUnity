using Core;
using Core.General;

namespace Board.Action
{
    public class Destroy: Action
    {
        public Destroy(int pos) : base(pos, false, false)
        {}

        public override void ApplyAction(GameState state)
        {
            
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(Caller);
        }

        public bool Roll(int chance)
        {
            return true;
        }
    }
}