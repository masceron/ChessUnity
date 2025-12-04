using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PhantomJellyActive: Action, ISkills
    {
        public int AIPenaltyValue => PieceOn(Target).Color != PieceOn(Maker).Color ? -30 : 0;
        public PhantomJellyActive(int maker) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            for (var i = -3; i <= 3; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -3; j <= 3; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);

                    if (p != null && p.Color != caller.Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, p)));
                    }
                }
            }
        }
    }
}