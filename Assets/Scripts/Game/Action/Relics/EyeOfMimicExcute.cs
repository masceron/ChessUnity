using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Others;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class EyeOfMimicExcute : Action, IRelicAction
    {
        public EyeOfMimicExcute(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            // apply 1 turn nhưng vì ApplyEffect tự động ++duration nên ở đây để là 0
            ActionManager.EnqueueAction(new ApplyEffect(new CopyCapturesMethod(Maker, Target , 0))); 
        }
    }
}
