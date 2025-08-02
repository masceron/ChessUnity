using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class LionfishActive: Action, ISkills
    {
        public LionfishActive(int caller) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)caller;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Caller);
            var caller = PieceOn(Caller);

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

            SetCooldown(Caller, ((IPieceWithSkill)PieceOn(Caller)).TimeToCooldown);
        }
    }
}