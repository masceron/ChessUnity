using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SwordFishActive: Action, ISkills
    {
        [MemoryPackConstructor]
        private SwordFishActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        public SwordFishActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(PieceOn(Maker), 1), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}