using Game.Action.Internal;
using Game.Common;
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

        public SeaStarResurrect(PieceLogic maker, int to) : base(maker, to)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMakerAsPiece();
            var collection = GetCapturedOf(caller.Color);

            ActionManager.EnqueueAction(new Resurrect(caller, new PieceConfig("piece_sea_star", caller.Color, GetTargetPos()),
                logic =>
                {
                    SetCooldown(logic, -1);
                }));
            
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}