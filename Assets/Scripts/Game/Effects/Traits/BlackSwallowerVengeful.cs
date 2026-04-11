using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Condition;
using Game.Piece.PieceLogic.Commons;
using Random = System.Random;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallowerVengeful : Vengeful
    {
        public BlackSwallowerVengeful(PieceLogic piece) : base(piece, VengefulType.OnDeath, "effect_black_swallower_vengeful")
        {
        }

        /// <summary>
        /// Nullify 1 quân địch ngẫu nhiên trong bán kính 1 ô.
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Piece, 1);

            if (listPieces.Count == 0) return;

            var random = new Random();
            var randomPiece = listPieces[random.Next(0, listPieces.Count)];

            if (randomPiece == null) return;
            
            ActionManager.EnqueueAction(new Nullify(Piece, randomPiece));
        }


        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}