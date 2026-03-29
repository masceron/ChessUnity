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
        [MemoryPackInclude] private int secondTarget;

        [MemoryPackConstructor]
        private EyeshadeSculpinActive()
        {
        }

        public EyeshadeSculpinActive(PieceLogic maker, PieceLogic firstTarget, PieceLogic secondTarget) : base(maker,
            firstTarget)
        {
            this.secondTarget = secondTarget.ID;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, GetTarget() as PieceLogic),
                GetMaker() as PieceLogic));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, GetEntityByID(secondTarget) as PieceLogic),
                GetMaker() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}