using MemoryPack;
using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SeaStarResurrect: Action, ISkills
    {
        [MemoryPackConstructor]
        private SeaStarResurrect() { }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public SeaStarResurrect(int maker, int to) : base(maker)
        {
            Target = to;
        }

        protected override void ModifyGameState()
        {
            var caller = PieceOn(Maker);
            var collection = !caller.Color ? WhiteCaptured() : BlackCaptured();
            
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig("piece_sea_star", caller.Color, Target)));
            collection.Remove(collection.First(p => p.Type == "piece_sea_star"));
            SetCooldown(Target, -1);
            SetCooldown(Maker, ((IPieceWithSkill) PieceOn(Maker)).TimeToCooldown);
        }
    }
}