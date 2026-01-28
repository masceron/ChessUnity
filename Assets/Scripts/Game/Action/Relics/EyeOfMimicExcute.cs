using System;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Common;
using Game.Effects.Others;
using UnityEngine;

namespace Game.Action.Relics
{
    public class EyeOfMimicExcute : Action, IRelicAction
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
