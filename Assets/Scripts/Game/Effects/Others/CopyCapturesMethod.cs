using Game.Piece.PieceLogic;
using Game.Movesets;

namespace Game.Effects.Others
{
    public class CopyCapturesMethod : Effect
    {
        private PieceLogic ourPiece;
        private PieceLogic opponentPiece;
        private byte ourPieceAttackRangeSaver;
        private CapturesDelegate ourPieceCapturesSaver;

        public CopyCapturesMethod(PieceLogic firstPiece, PieceLogic secondPiece, sbyte duration) : base(duration, 1, firstPiece, EffectName.CopyCaptureMethod)
        {
            Duration = duration;
            ourPiece = firstPiece;
            opponentPiece = secondPiece;
        }

        public override void OnApply()
        {
            if (ourPiece == null || opponentPiece == null) return;

            ourPieceAttackRangeSaver = ourPiece.AttackRange;
            ourPiece.AttackRange = opponentPiece.AttackRange;

            ourPieceCapturesSaver = ourPiece.Captures;
            ourPiece.Captures = opponentPiece.Captures;
        }

        public override void OnRemove()
        {
            if (ourPiece != null)
            {
                ourPiece.AttackRange = ourPieceAttackRangeSaver;
                ourPiece.Captures = ourPieceCapturesSaver;
            }
        }
    }
}

