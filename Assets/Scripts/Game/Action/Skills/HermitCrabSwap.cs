using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrabSwap: Action, ISkills
    {
        public HermitCrabSwap(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }
        protected override void Animate()
        {
           PieceManager.Ins.Swap(Maker, Target);
        }
        protected override void ModifyGameState()
        {
            // var board = PieceBoard();
            // int a = Maker;
            // int b = Target;

            // var pieceA = board[(ushort)a];
            // var pieceB = board[(ushort)b];

            // board[(ushort)a] = pieceB;
            // if (pieceB != null) pieceB.Pos = (ushort)a;
            // board[(ushort)b] = pieceA;
            // if (pieceA != null) pieceA.Pos = (ushort)b;
            // SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);

            MatchManager.Ins.GameState.Swap(Maker, Target);
            SetCooldown(Target, ((IPieceWithSkill)PieceOn(Target)).TimeToCooldown);
        }
    }
}