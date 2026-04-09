using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThreadPipefish : Commons.PieceLogic
    {
        public ThreadPipefish(PieceConfig cfg) : base(cfg, UpDoorMoves.Quiets, UpDoorMoves.Captures)
        {
         //Làm lại  
        }
    }
}