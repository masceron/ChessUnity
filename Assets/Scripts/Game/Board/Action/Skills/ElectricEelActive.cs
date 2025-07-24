using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using static Game.Board.General.MatchManager;
using Game.Board.Piece.PieceLogic.Elites;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class ElectricEelActive: Action, ISkills
    {
        public ElectricEelActive(int caller) : base(caller, false)
        {
            To = (ushort)caller;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Caller);
            var caller = gameState.MainBoard[Caller];

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = gameState.MainBoard[IndexOf(rankOff, fileOff)];
                    if (p == null || p.color == caller.color) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, p)));
                }
            }

            ((ElectricEel)caller).SkillCooldown = 8;
        }
    }
}