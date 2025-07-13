using Game.Board.General;
using Game.Board.Piece;
using Game.Board.PieceLogic;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action
    {
        private readonly PieceManager pieceManager;
        public SirenActive(ushort caller, int f, int t, PieceManager p) : base(caller, true, false, false)
        {
            From = (ushort)f;
            To = (ushort)t;
            pieceManager = p;
        }
        public override void ApplyAction(GameState state)
        {
            pieceManager.Move(From, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Move(From, To);
            state.MainBoard[To].color = state.MainBoard[To].color == Color.White ? Color.Black : Color.White;
            ((GuidingSiren) state.MainBoard[Caller]).SkillCooldown = 12;
        }
    }
}