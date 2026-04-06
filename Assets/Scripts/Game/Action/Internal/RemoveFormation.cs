using Game.Common;
using Game.Tile;

namespace Game.Action.Internal
{
    public class RemoveFormation: Action, IInternal
    {
        private readonly Formation _toRemove;

        public RemoveFormation(Formation formation)
        {
            _toRemove = formation;
        }
        
        protected override void ModifyGameState()
        {
            BoardUtils.RemoveFormation(_toRemove);
        }
    }
}