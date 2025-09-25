using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using System;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcherfishActive: Action, ISkills
    {
        public ArcherfishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

/*        protected override void Animate()
        {

        }*/

        protected override void ModifyGameState()
        {
            // Gây hiệu ứng Blind và Marked lên 1 quân địch trong bán kính 4 ô
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);
            var enemyPieces = GetPiecesInRange(rank, file, 4, p => p != null && p.Color != caller.Color);
            if (enemyPieces.Count > 0)
            {
                var rand = new Random();
                var target = enemyPieces[rand.Next(0, enemyPieces.Count)];
                ActionManager.EnqueueAction(new ApplyEffect(new Blinded(1, 100, target))); //100% blind
                //ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, target)));
            }

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}

