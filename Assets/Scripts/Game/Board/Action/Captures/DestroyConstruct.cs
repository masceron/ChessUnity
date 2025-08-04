using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    public class DestroyConstruct: Action, ICaptures
    {
        public DestroyConstruct(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Maker);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Destroy(Maker);
            MatchManager.Ins.GameState.Destroy(Target);
        }
    }
}