using System.Linq;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Broken: Effect
    {
        public Broken(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_broken")
        {}

        private Effect specialAbility;
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }

        public override void OnApply()
        {
            specialAbility = Piece.Effects.FirstOrDefault(e => e.Category == EffectCategory.SpecialAbility);
            if (specialAbility != null)
            {
                specialAbility.disabled = true;
            }
        }

        public override void OnRemove()
        {
            if (specialAbility != null)
            {
                specialAbility.disabled = false;
            }
        }
    }
}