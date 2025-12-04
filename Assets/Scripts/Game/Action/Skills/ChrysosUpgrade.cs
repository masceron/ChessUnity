using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosUpgrade: Action, ISkills
    {
        public int AIPenaltyValue => 0;

        private readonly PieceConfig target;
        private readonly byte cost;

        public ChrysosUpgrade(int maker, PieceConfig t, byte cost) : base(maker)
        {
            target = t;
            this.cost = cost;
        }

        protected override void ModifyGameState()
        {
            var pieceOn = PieceOn(Maker);
            ActionManager.EnqueueAction(new DestroyPiece(target.Index));
            ActionManager.EnqueueAction(new SpawnPiece(target));
            ((Chrysos)pieceOn).Coin -= cost;
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}