using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Common;
using static Game.Common.BoardUtils;

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
            var pieceOn = BoardUtils.PieceOn(Caller);
            ActionManager.EnqueueAction(new DestroyPiece(target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(target));
            ((Chrysos)pieceOn).Coin -= cost;
            SetCooldown(Caller, ((IPieceWithSkill)PieceOn(Caller)).TimeToCooldown);
        }
    }
}