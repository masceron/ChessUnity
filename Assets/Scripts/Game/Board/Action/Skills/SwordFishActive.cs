using Game.Board.Action.Internal;
using Game.Board.Effects.Traits;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class SwordFishActive: Action, ISkills
    {
        public SwordFishActive(int caller) : base(caller, true)
        {
            From = (ushort)caller;
            To = (ushort)caller;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(PieceOn(Caller), 2)));
            SetCooldown(Caller, ((IPieceWithSkill)PieceOn(Caller)).TimeToCooldown);
        }
    }
}