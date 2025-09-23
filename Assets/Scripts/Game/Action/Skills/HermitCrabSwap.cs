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
            PieceManager.Ins.Move(Maker, Target);
            PieceManager.Ins.Move(Target, Maker);
        }
        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(Maker, Target);
            MatchManager.Ins.GameState.Move(Target, Maker);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}