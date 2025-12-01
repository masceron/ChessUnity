using Game.Action;
using Game.Action.Internal;
using Game.Common;
using static Game.Common.BoardUtils;
using System.Collections.Generic;
using System.Linq;
using System;
using Game.Piece;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class  BlackSwallowerVengeful: Effect, IDeadEffect
    {
        public BlackSwallowerVengeful(PieceLogic piece) : base(-1, 1, piece, "effect_black_swallower_vengeful")
        {
        }

        public void OnCallDead()
        {
            UnityEngine.Debug.Log("BlackSwallowerVengeful OnCallDead");
            var pieceAround = new List<PieceLogic>();
            foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Piece.Pos), FileOf(Piece.Pos), 1))
            {
                var index = IndexOf(rank, file);
                var pOn = PieceOn(index);
                if (pOn == null || pOn == Piece) continue;
                pieceAround.Add(pOn);
            }
            if (pieceAround.Count == 0) return;
            var random = new Random();
            var randomPiece = pieceAround[random.Next(0, pieceAround.Count)];
            if (randomPiece == null) return;
            foreach (var effect in randomPiece.Effects.Where(effect => effect.Category == EffectCategory.Buff))
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}