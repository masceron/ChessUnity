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

        public ChrysosUpgrade(int from, PieceConfig t, byte cost) : base(from, false)
        {
            target = t;
            this.cost = cost;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = BoardUtils.PieceOn(From);
            ActionManager.EnqueueAction(new DestroyPiece(target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(target));
            ((Chrysos)pieceOn).Coin -= cost;
            SetCooldown(From, ((IPieceWithSkill)PieceOn(From)).TimeToCooldown);
        }
    }
}