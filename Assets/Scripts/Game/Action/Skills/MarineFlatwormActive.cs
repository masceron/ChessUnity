using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MarineFlatwormActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MarineFlatwormActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        public MarineFlatwormActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        protected override void ModifyGameState()
        {
            var config = new PieceConfig(PieceOn(Maker).Type, PieceOn(Maker).Color, Target);
            ActionManager.EnqueueAction(new SpawnPieceWithEffect(config, new Illusion(PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}