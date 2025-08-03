using Game.Board.Action.Internal;
using Game.Board.Effects.Traits;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class SwordFishActive: Action, ISkills
    {
        public SwordFishActive(int from) : base(from, true)
        {
            From = (ushort)from;
            To = (ushort)from;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(PieceOn(From), 1)));
            SetCooldown(From, ((IPieceWithSkill)PieceOn(From)).TimeToCooldown);
        }
    }
}