using Game.Common;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EndTurn: Action
    {
        public EndTurn(): base(-1)
        {
            Maker = 0;
            Target = 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            BoardUtils.FlipSideToMove();
        }
    }
}