using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineFlatwormActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        public MarineFlatwormActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        protected override void ModifyGameState()
        {
            var config = new PieceConfig(PieceOn(Maker).Type, PieceOn(Maker).Color, (ushort)Target);
            ActionManager.EnqueueAction(new SpawnPieceWithEffect(config, new Illusion(PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}