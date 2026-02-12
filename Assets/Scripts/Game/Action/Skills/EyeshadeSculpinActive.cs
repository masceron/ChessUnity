using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class EyeshadeSculpinActive : Action, ISkills
    {
        [MemoryPackInclude]
        private readonly int firstTargetPos;
        [MemoryPackInclude]
        private readonly int secondTargetPos;
        
        public EyeshadeSculpinActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            firstTargetPos = firstTarget;
            secondTargetPos = secondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, PieceOn(firstTargetPos)), PieceOn(Maker)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, PieceOn(secondTargetPos)), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
        
    }
}