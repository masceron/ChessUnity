using Game.Action.Internal;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;
namespace Game.Action.Relics
{

    public class MangroveCharmExcute : Action, IRelicAction
    {
        private int FirstTarget;
        private int SecondTarget;

        public MangroveCharmExcute(int FirstTarget, int SecondTarget) : base(-1)
        {
            this.FirstTarget = FirstTarget;
            this.SecondTarget = SecondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(FirstTarget))));
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(SecondTarget))));

        }
    }
}
