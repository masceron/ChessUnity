using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class CopyCapturesMethod : Effect
    {
        private readonly PieceLogic ourPiece;
        private readonly PieceLogic opponentPiece;
        private byte ourPieceAttackRangeSaver;
        private CapturesDelegate ourPieceCapturesSaver;

        public CopyCapturesMethod(PieceLogic firstPiece, PieceLogic secondPiece, sbyte duration) : base(duration, 1, firstPiece, "effect_copy_captures_method")
        {
            Duration = duration;
            ourPiece = firstPiece;
            opponentPiece = secondPiece;
        }

        public override void OnApply()
        {
            if (ourPiece == null || opponentPiece == null) return;

            ourPieceAttackRangeSaver = ourPiece.AttackRange();
            ourPiece.SetAttackRange(opponentPiece.AttackRange());

            ourPieceCapturesSaver = ourPiece.Captures;
            ourPiece.Captures = opponentPiece.Captures;
        }

        public override void OnRemove()
        {
            if (ourPiece != null)
            {
                ourPiece.SetAttackRange(ourPieceAttackRangeSaver);
                ourPiece.Captures = ourPieceCapturesSaver;
            }
        }
    }
}

