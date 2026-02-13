using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.Effects.Triggers;

namespace Game.Effects.Others
{
    public class CopyCapturesMethod : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        private readonly PieceLogic _ourPiece;
        private readonly PieceLogic _opponentPiece;
        private int _ourPieceAttackRangeSaver;
        private CapturesDelegate _ourPieceCapturesSaver;

        public CopyCapturesMethod(int firstPiece, int secondPiece, int duration) : base(duration, 1, BoardUtils.PieceOn(firstPiece), "effect_copy_captures_method")
        {
            Duration = duration;
            _ourPiece = BoardUtils.PieceOn(firstPiece);
            _opponentPiece = BoardUtils.PieceOn(secondPiece);
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
