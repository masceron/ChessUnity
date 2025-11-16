using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using System.Linq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnaggletoothsActive: Action, ISkills
    {
        private bool flag;
        public SnaggletoothsActive(int maker, int to, bool flag) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            this.flag = flag;
        }
        protected override void ModifyGameState()
        {
            if (!flag)
            {
                var existingBleeding = PieceOn(Target).Effects.OfType<Bleeding>().ToList();

                foreach (var bleeding in existingBleeding)
                {
                    ActionManager.ExecuteImmediately(new RemoveEffect(bleeding));
                }
            } 
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(Maker))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}