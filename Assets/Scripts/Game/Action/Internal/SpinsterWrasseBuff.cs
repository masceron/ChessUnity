using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class SpinsterWrasseBuff : Action, IInternal
    {
        private readonly int _firstTarget;
        private readonly int _secondTarget;

        public SpinsterWrasseBuff(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            _secondTarget = secondTarget;
            _firstTarget = firstTarget;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);

            ActionManager.EnqueueAction(new Purify(GetMakerPos(), _firstTarget));
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(PieceOn(_secondTarget)), GetMaker() as PieceLogic));
        }
    }
}