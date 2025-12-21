using Game.Managers;
using UnityEngine;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class IllusionCapture: Action, ICaptures
    {
        public IllusionCapture(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, PieceOn(Target))));
        }
    }
}