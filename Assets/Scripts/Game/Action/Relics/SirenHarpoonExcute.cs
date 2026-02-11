using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Action.Relics
{
    public class SirenHarpoonExcute : Action, IRelicAction
    {
        public SirenHarpoonExcute(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Controlled(-1, BoardUtils.PieceOn(Target))));
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(1, BoardUtils.PieceOn(Target))));
        }
    }
}
