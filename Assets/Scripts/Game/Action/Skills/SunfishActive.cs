using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunfishActive : Action, ISkills
    {
        public int AIPenaltyValue => PieceOn(Target).Color != PieceOn(Maker).Color ? -10 : 0;
        private readonly int Range;
        public SunfishActive(int maker,int range) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
            Range = range;  
        }

        protected override void ModifyGameState()   
        {
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

            for (var rankOff = rank - Range; rankOff <= rank + Range; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - Range; fileOff <= file + Range; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color == caller.Color || !ColorOfSquare(IndexOf(rankOff, fileOff))) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, p)));
                }
            }
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}