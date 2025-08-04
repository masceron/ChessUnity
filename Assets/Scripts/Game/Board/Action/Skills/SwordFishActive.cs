using Game.Board.Action.Internal;
using Game.Board.Effects.Traits;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class SwordFishActive: Action, ISkills
    {
        public SwordFishActive(int maker) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(PieceOn(Maker), 1)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}