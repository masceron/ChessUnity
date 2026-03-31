using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class GoldenBassletActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private GoldenBassletActive() { }

        public GoldenBassletActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            ActionManager.EnqueueAction(
                new ApplyEffect(new Blinded(maker.GetStat(SkillStat.Duration), 100, GetTargetAsPiece()),
                    maker)); // TODO: Check probability.
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)maker).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}