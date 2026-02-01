using Game.Action.Internal;
using Game.Piece;

namespace Game.Action.Relics
{
    public class CoralTomeAction : Action, IRelicAction
    {
        private string pieceType;
        private bool relicColor;
        
        public CoralTomeAction(bool color, string type, int maker) : base(maker)
        {
            relicColor = color;
            pieceType = type;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(pieceType, relicColor , (ushort)Maker)));
        }
    }
}