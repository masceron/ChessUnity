using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RedCopepodActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private RedCopepodActive()
        {
        }

        public RedCopepodActive(int maker, int target) : base(maker)
        {
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
            //SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}