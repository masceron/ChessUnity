using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;
using Random = System.Random;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallowerVengeful : Effect, IDeadTrigger
    {
        public BlackSwallowerVengeful(PieceLogic piece) : base(-1, 1, piece, "effect_black_swallower_vengeful")
        {
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie != Piece) return;
            Debug.Log("BlackSwallowerVengeful OnCallDead");
            var pieceAround = new List<PieceLogic>();
            var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Piece.Pos, 1);
            foreach (var piece in listPieces) pieceAround.Add(PieceOn(piece));
            if (pieceAround.Count == 0) return;
            var random = new Random();
            var randomPiece = pieceAround[random.Next(0, pieceAround.Count)];
            if (randomPiece == null) return;
            // foreach (var effect in randomPiece.Effects.Where(effect => effect.Category == EffectCategory.Buff))
            // {
            //     ActionManager.EnqueueAction(new RemoveEffect(effect));
            // }
            ActionManager.EnqueueAction(new Nullify(Piece.Pos, randomPiece.Pos));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}