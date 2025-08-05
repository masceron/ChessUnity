using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
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