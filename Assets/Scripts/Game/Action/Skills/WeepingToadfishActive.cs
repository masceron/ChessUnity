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
    public partial class WeepingToadfishActive : Action, ISkills
    {
        private int Duration;
        [MemoryPackConstructor]
        private WeepingToadfishActive()
        {
        }

        public WeepingToadfishActive(PieceLogic maker, PieceLogic target, int duration) : base(maker, target)
        {
            Duration = duration;
        }

        protected override void Animate()
        {
        }
        public int AIPenaltyValue(PieceLogic maker) { return -20; }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(Duration, GetTargetAsPiece())));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}