using Game.Effects;
using Game.Piece.PieceLogic;
using Game.Movesets;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Effects.Others
{
    public class SwapMoveMethod : Effect
    {
        private PieceLogic fP;
        private PieceLogic sP;
        private bool canSwap = false;
        public SwapMoveMethod(PieceLogic firstPiece, PieceLogic secondPiece, sbyte duration, bool canSwap) : base(duration, 1, firstPiece, EffectName.SwapMoveMethod)
        {
            Duration = duration;
            fP = firstPiece;
            sP = secondPiece;
            this.canSwap = canSwap;
        }

        public override void OnApply()
        {
            SwapMove();
        }

        public override void OnRemove()
        {
            SwapMove();
        }

        private void SwapMove()
        {
            if (fP == null || sP == null || !canSwap) return;

            List<byte> tempMoveRange = new List<byte>();
            tempMoveRange.AddRange(fP.MoveRange);
            fP.MoveRange.Clear();
            fP.MoveRange.AddRange(sP.MoveRange);
            sP.MoveRange.Clear();
            sP.MoveRange.AddRange(tempMoveRange);

            QuietsDelegate temp = fP.Quiets;
            fP.Quiets = sP.Quiets;
            sP.Quiets = temp;

        }
    }
}

