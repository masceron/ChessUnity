using Game.Managers;
using UX.UI.Ingame;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Internal
{
    public class MarinelKill : Action, ICaptures
    {
        private readonly int targetPos;
        
        public MarinelKill(int maker, int target, int to) : base(maker)
        {
            Target = (ushort)target;
            targetPos = to;
        }
        
        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);

            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Kill(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
            ActionManager.EnqueueAction(new DestroyPiece(targetPos));
            BoardViewer.ListOf.Clear();
        }
        

    }
}