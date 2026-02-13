using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class SpinsterWrasseBuff : Action, IInternal
    {
        private readonly int firstTarget;
        private readonly int secondTarget;

        public SpinsterWrasseBuff(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = maker;
            this.secondTarget = secondTarget;
            this.firstTarget = firstTarget;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);

            ActionManager.EnqueueAction(new Purify(Maker, firstTarget));
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(PieceOn(secondTarget)), PieceOn(Maker)));
        }
    }
}