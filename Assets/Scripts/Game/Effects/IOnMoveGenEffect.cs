using System.Collections.Generic;

namespace Game.Effects
{
    public interface IOnMoveGenEffect
    {
        public void OnCallMoveGen(List<Action.Action> actions);
    }
}