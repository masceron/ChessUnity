using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SohalSurgeonfishActive : Action, ISkills
    {
        private readonly int duration;
        [MemoryPackConstructor]
        private SohalSurgeonfishActive()
        {
        }

        public SohalSurgeonfishActive(PieceLogic maker, PieceLogic target, int duration) : base(maker, target)
        {
            this.duration = duration;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Leashed(GetTargetAsPiece(), duration), GetMakerAsPiece()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}