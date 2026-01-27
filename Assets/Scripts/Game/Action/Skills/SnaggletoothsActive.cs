using Game.Action.Internal;
using Game.Effects.Debuffs;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.AI;
using Game.Effects.Buffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnaggletoothsActive: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color == maker.Color ? 10 : 0;
        }
        public SnaggletoothsActive(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }
        protected override void ModifyGameState()
        {

            var existingBleeding = PieceOn(Target).Effects.OfType<Bleeding>().ToList();

            foreach (var bleeding in existingBleeding)
            {
                ActionManager.EnqueueAction(new RemoveEffect(bleeding));
            }

            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(Maker), 5)));

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}