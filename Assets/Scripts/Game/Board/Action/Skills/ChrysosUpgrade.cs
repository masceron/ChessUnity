using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic.Commanders;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Skills
{
    public class ChrysosUpgrade: Action, ISkills
    {
        private readonly PieceConfig target;
        private readonly byte cost;

        public ChrysosUpgrade(int caller, PieceConfig t, byte cost) : base(caller, false)
        {
            target = t;
            this.cost = cost;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new DestroyPiece(target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(target));
            ((Chrysos)gameState.MainBoard[Caller]).Coin -= cost;
            ((Chrysos)gameState.MainBoard[Caller]).SkillCooldown = 4;
        }
    }
}