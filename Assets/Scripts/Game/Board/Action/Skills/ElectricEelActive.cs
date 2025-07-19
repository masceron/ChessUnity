using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Game.Board.Piece.PieceLogic.Elites;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class ElectricEelActive: Action, ISkills
    {
        public ElectricEelActive(int caller) : base(caller, false)
        {
            
        }

        public override void ApplyAction(GameState state)
        {
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var (rank, file) = RankFileOf(Caller);
            var caller = state.MainBoard[Caller];

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = MatchManager.GameState.MainBoard[IndexOf(rankOff, fileOff)];
                    if (p == null || p.color == caller.color) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, p)));
                }
            }

            ((ElectricEel)caller).SkillCooldown = 8;
        }
    }
}