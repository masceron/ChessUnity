using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class LionfishActive: Action, ISkills
    {
        public LionfishActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color == caller.Color) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
                }
            }

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}