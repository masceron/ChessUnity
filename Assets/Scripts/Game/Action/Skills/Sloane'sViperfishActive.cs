using System.Linq;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class SloaneSViperfishActive : Action, ISkills
    {
        public SloaneSViperfishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            for (var i = -4; i <= 4; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -4; j <= 4; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);
                    if (p == caller || p.Color == caller.Color) continue;
                    if (p != null)
                    {
                        bool bleeding = false;
                        for (int e = 0; e < p.Effects.Count; e++)
                        {
                            if (p.Effects[e].EffectName == EffectName.Bleeding)
                            {
                                bleeding = true;
                            }
                        }
                        var bleeding = p.Effects.Any(t => t.EffectName == "effect_bleeding");

                    ActionManager.ExecuteImmediately(bleeding
                        ? new ApplyEffect(new Poison(1, p))
                        : new ApplyEffect(new Bleeding(5, p)));
                }
            }
        }
    }
}