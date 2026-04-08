using Game.Common;
using Game.Tile;

namespace Game.Action.Internal
{
    public class SetFormation : Action, IInternal
    {
        private readonly int _pos;
        private readonly Formation _formation;

        public SetFormation(int pos, Formation formation)
        {
            _pos = pos;
            _formation = formation;
        }

        protected override void ModifyGameState()
        {
            BoardUtils.SetFormation(_pos, _formation);
        }
    }
}
