using System.Linq;
using static Game.Common.BoardUtils;
using Game.Action.Internal;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BarnacleActive: Action, ISkills
    {
        public BarnacleActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void Animate()
        {

        }

        protected override void ModifyGameState()
        {
            foreach (var effect in PieceOn(Target).Effects
                         .Where(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield"))
            {
                if (effect.Duration > 0)
                    effect.Duration -= 1;
                else
                {
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
                }
            }
            
            SetCooldown(Maker, -1);
            //SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}