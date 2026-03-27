using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class MarinelKill : Action, IInternal
    {
        private readonly int firstTarget;
        private readonly int secondTarget;

        public MarinelKill(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            this.firstTarget = firstTarget;
            this.secondTarget = secondTarget;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);

            ActionManager.EnqueueAction(
                new ApplyEffect(new MarineIguanaKillEffect(0, GetTarget(), GetFrom(), secondTarget), GetMaker()));
        }
    }
}