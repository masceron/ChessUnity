using Game.Action.Internal;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ThalassosResurrect : Action, ISkills
    {
        [MemoryPackInclude] private string typeTo;

        [MemoryPackConstructor]
        private ThalassosResurrect()
        {
        }

        public ThalassosResurrect(int maker, int to, string typeTo) : base(maker)
        {
            Maker = maker;
            Target = to;
            this.typeTo = typeTo;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            var color = ColorOfPiece(Maker);
            var collection = !color ? gameState.WhiteCaptured : gameState.BlackCaptured;
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(typeTo, color, Target)));

            collection.Remove(collection.First(e => e.Type == typeTo));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}