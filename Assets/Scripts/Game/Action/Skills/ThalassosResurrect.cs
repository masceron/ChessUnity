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
        [MemoryPackInclude] private string _typeTo;

        [MemoryPackConstructor]
        private ThalassosResurrect()
        {
        }

        public ThalassosResurrect(PieceLogic maker, int to, string typeTo) : base(maker, to)
        {
            this._typeTo = typeTo;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var color = GetMakerAsPiece().Color;
            var collection = !color ? WhiteCaptured() : BlackCaptured();
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(_typeTo, color, GetTargetPos())));

            collection.Remove(collection.First(e => e.Type == _typeTo));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}