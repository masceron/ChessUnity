using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Others;
using Game.Effects.Buffs;
namespace Game.Action.Internal
{
    public class SpinsterWrasseBuff : Action, IInternal
    {
        private readonly int secondTarget;
        private readonly int firstTarget;

        public SpinsterWrasseBuff(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
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