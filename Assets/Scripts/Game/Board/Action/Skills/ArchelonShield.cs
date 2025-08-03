using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class ArchelonShield: Action, ISkills
    {
        public ArchelonShield(int from, int to) : base(from, false)
        {
            From = (ushort)from;
            To = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(To))));
            SetCooldown(From, ((IPieceWithSkill)PieceOn(From)).TimeToCooldown);
        }
    }
}