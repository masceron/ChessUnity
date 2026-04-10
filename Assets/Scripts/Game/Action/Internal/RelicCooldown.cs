using Game.Common;

namespace Game.Action.Internal
{
    public class RelicCooldown: Action, IInternal
    {
        protected override void ModifyGameState()
        {
            BoardUtils.SetRelicCooldown(BoardUtils.GetRelicOf(BoardUtils.SideToMove()));
        }
    }
}