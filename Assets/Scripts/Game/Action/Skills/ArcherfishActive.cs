using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic;
using Game.Action.Internal;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcherfishActive: Action, ISkills
    {
        public ArcherfishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void Animate()
        {

        }

        protected override void ModifyGameState()
        {
            // Gây hiệu ứng Blind và Marked lên 1 quân địch trong bán kính 4 ô
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, PieceOn(Target))));
            //ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, PieceOn(Target))));

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}

