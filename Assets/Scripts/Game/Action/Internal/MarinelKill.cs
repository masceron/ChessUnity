using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class MarinelKill : Action, IInternal
    {
        private readonly int secondTarget;

        public MarinelKill(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Target = firstTarget;
            this.secondTarget = secondTarget;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);

            ActionManager.EnqueueAction(
                new ApplyEffect(new MarineIguanaKillEffect(0, PieceOn(Target), Maker, secondTarget), PieceOn(Maker)));
        }
    }
}