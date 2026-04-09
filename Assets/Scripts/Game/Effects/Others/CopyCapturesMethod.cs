using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class CopyCapturesMethod : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        private readonly PieceLogic _opponentPiece;
        private readonly PieceLogic _ourPiece;
        private int _ourPieceAttackRangeSaver;
        private CapturesDelegate _ourPieceCapturesSaver;

        public CopyCapturesMethod(PieceLogic firstPiece, PieceLogic secondPiece, int duration) : base(duration, 1,
            firstPiece, "effect_copy_captures_method")
        {
            Duration = duration;
            _ourPiece = firstPiece;
            _opponentPiece = secondPiece;
        }

        public void OnApply()
        {
            if (_ourPiece == null || _opponentPiece == null) return;

            _ourPieceAttackRangeSaver = _ourPiece.AttackRange();
            _ourPiece.SetAttackRange(_opponentPiece.AttackRange());

            _ourPieceCapturesSaver = _ourPiece.Captures;
            _ourPiece.Captures = _opponentPiece.Captures;
        }

        public void OnRemove()
        {
            if (_ourPiece != null)
            {
                _ourPiece.SetAttackRange(_ourPieceAttackRangeSaver);
                _ourPiece.Captures = _ourPieceCapturesSaver;
            }
        }
    }
}