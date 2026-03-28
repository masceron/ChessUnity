using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnaggletoothsActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SnaggletoothsActive()
        {
        }

        public SnaggletoothsActive(int maker, int to) : base(maker)
        {
            Maker = maker;
            Target = to;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null) return 0;
            return pieceAI.Color == maker.Color ? 10 : 0;
        }

        protected override void ModifyGameState()
        {
            var existingBleeding = GetTarget().Effects.OfType<Bleeding>().ToList();

            foreach (var bleeding in existingBleeding) ActionManager.EnqueueAction(new RemoveEffect(bleeding));

            ActionManager.EnqueueAction(new ApplyEffect(new Shield(GetMaker(), 5)));

            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}