using Game.Action.Internal;

namespace Game.Action.Relics
{
    public class RottingScytheAction : Action, IRelicAction
    {
        public RottingScytheAction(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Maker));
        }
    }
}