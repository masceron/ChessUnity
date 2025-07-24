using Game.Board.General;
using static Game.Board.General.MatchManager;
using Game.Board.Piece.PieceLogic.Commanders;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action, ISkills
    { public SirenActive(ushort caller, int f, int t) : base(caller, true)
        {
            From = (ushort)f;
            To = (ushort)t;
        }
        protected override void Animate()
        {
            pieceManager.Move(From, To);
        }

        protected override void ModifyGameState()
        {
            gameState.Move(From, To);
            gameState.MainBoard[To].color = gameState.MainBoard[To].color == Color.White ? Color.Black : Color.White;
            ((GuidingSiren) gameState.MainBoard[Caller]).SkillCooldown = 12;
        }
    }
}