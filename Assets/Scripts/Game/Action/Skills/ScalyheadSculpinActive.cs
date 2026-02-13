using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class ScalyheadSculpinActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ScalyheadSculpinActive() { }

        private const int carapaceDuration = 4;
        public ScalyheadSculpinActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(carapaceDuration, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}