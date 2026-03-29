using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SeaStarResurrect : Action, ISkills
    {
        [MemoryPackConstructor]
        private SeaStarResurrect()
        {
        }

        public SeaStarResurrect(int maker, int to) : base((PieceLogic)maker, (PieceLogic)to)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMaker() as PieceLogic;
            var collection = !caller.Color ? WhiteCaptured() : BlackCaptured();

            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig("piece_sea_star", caller.Color, GetTargetPos()),
                logic =>
                {
                    SetCooldown(logic, -1);
                }));
            
            collection.Remove(collection.First(p => p.Type == "piece_sea_star"));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}