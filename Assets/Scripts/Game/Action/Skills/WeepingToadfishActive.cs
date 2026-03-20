using Game.Action.Internal;
using Game.Action.Quiets;
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

        public WeepingToadfishActive(int maker, int target, int duration) : base(maker)
        {
            Target = target;
            Duration = duration;
        }

        protected override void Animate()
        {
        }
        public int AIPenaltyValue(PieceLogic maker) { return -20; }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(Duration, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}