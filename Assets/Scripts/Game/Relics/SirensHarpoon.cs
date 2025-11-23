using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Piece;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirensHarpoon : RelicLogic
    {
        public SirensHarpoon(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            currentCooldown = 0;
        }
        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;


                    if (piece.Color != Color && piece.PieceRank <= PieceRank.Common)
                    {
                        TileManager.Ins.MarkAsMoveable(piece.Pos);
                        var pending = new SirensHarpoonPending(this, piece.Pos, piece.Color);
                        BoardViewer.ListOf.Add(pending);
                    }
                    BoardViewer.Selecting = -2;
                    BoardViewer.SelectingFunction = 4;
                }
            }

        }
    }
}