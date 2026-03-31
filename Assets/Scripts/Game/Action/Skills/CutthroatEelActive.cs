using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class CutthroatEelActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private CutthroatEelActive()
        {
        }

        public CutthroatEelActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = GetMakerAsPiece();
            var direction = makerPiece.Color ? 1 : -1;
            ActionManager.EnqueueAction(new NormalMove(GetMakerAsPiece(), 
                IndexOf(RankOf(GetTargetPos()) + direction, FileOf(GetTargetPos()))));
            var bleeding = GetTargetAsPiece().Effects.First(e => e is Bleeding);
            if (bleeding.Strength >= 4)
            {
                bleeding.Strength -= 1;
            }
            else
            {
                ActionManager.EnqueueAction(new KillPiece(GetTargetAsPiece()));
                SetFormation(makerPiece.Pos, new FogOfWar(makerPiece.Color));
            }
        }
    }
}