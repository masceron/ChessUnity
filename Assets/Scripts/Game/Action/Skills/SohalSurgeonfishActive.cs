using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfishActive: Action, ISkills
    {
        public SohalSurgeonfishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Leashed(PieceOn(Target), Target, 5)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}