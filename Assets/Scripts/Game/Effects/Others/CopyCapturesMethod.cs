using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Common;

namespace Game.Effects.Others
{
    public class CopyCapturesMethod : Effect, IOnApply, IOnRemove
    {
        private readonly PieceLogic ourPiece;
        private readonly PieceLogic opponentPiece;
        private byte ourPieceAttackRangeSaver;
        private CapturesDelegate ourPieceCapturesSaver;

        public CopyCapturesMethod(int firstPiece, int secondPiece, sbyte duration) : base(duration, 1, BoardUtils.PieceOn(firstPiece), "effect_copy_captures_method")
        {
            Duration = duration;
            ourPiece = BoardUtils.PieceOn(firstPiece);
            opponentPiece = BoardUtils.PieceOn(secondPiece);
        }

        public void OnApply()
        {
            if (ourPiece == null || opponentPiece == null) return;

            ourPieceAttackRangeSaver = ourPiece.AttackRange();
            ourPiece.SetAttackRange(opponentPiece.AttackRange());

            ourPieceCapturesSaver = ourPiece.Captures;
            ourPiece.Captures = opponentPiece.Captures;
        }

        public void OnRemove()
        {
            if (ourPiece != null)
            {
                ourPiece.SetAttackRange(ourPieceAttackRangeSaver);
                ourPiece.Captures = ourPieceCapturesSaver;
            }
        }
    }
}
