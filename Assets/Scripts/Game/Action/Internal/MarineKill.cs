using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    public class MarineKill : Action
    {
        private readonly int secondTarget;

        public MarineKill(PieceLogic maker, PieceLogic firstTarget, PieceLogic secondTarget) : base(maker, firstTarget)
        {
            this.secondTarget = secondTarget.ID;
        }

        protected override void ModifyGameState()
        {
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);

            ActionManager.EnqueueAction(
                new ApplyEffect(
                    new MarineIguanaKillEffect(0, GetMaker() as PieceLogic, GetTarget() as PieceLogic, GetEntityByID(secondTarget)),
                    GetMaker() as PieceLogic));
        }
    }
}