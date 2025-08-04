using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class ArchelonShield: Action, ISkills
    {
        public ArchelonShield(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}