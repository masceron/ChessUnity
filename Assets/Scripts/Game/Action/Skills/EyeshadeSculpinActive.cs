using System;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class EyeshadeSculpinActive : Action, ISkills
    {
        [MemoryPackInclude] private int firstTargetPos;

        [MemoryPackInclude] private int secondTargetPos;

        [MemoryPackConstructor]
        private EyeshadeSculpinActive()
        {
        }

        public EyeshadeSculpinActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            firstTargetPos = firstTarget;
            secondTargetPos = secondTarget;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, PieceOn(firstTargetPos)), GetMaker()));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, PieceOn(secondTargetPos)),
                GetMaker()));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}