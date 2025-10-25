using Game.Managers;
using UX.UI.Ingame;
namespace Game.Action.Internal
{
    public class KillPieceAction : Action
    {
        private readonly int targetPos;
        
        public KillPieceAction(int targetPos) : base(-1)
        {
            this.targetPos = targetPos;
            Target = (ushort)targetPos;  
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new KillPiece(targetPos));
            BoardViewer.Ins.Unmark();
        }
        

    }
}