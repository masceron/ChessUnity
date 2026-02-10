using Game.Tile;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Quiets;
using Game.Action.Internal;

namespace Game.Action.Skills
{
    public class CutthroatEelActive  : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public CutthroatEelActive(int maker, int target) : base(maker)
        {
            Target = target; // Target is enemy
        }
        protected override void ModifyGameState()
        {
            var makerPiece = PieceOn(Maker);
            var direction = PieceOn(Maker).Color ? 1 : -1;
            ActionManager.EnqueueAction(new NormalMove(Maker, IndexOf(RankOf(Target) + direction, FileOf(Target))));
            var bleeding = PieceOn(Target).Effects.First(e => e is Bleeding);
            if (bleeding.Strength >= 4)
            {
                bleeding.Strength -= 1;
            }
            else
            {
                ActionManager.EnqueueAction(new KillPiece(Target));
                SetFormation(makerPiece.Pos, new FogOfWar(makerPiece.Color));
            }
        }
        
    }
}