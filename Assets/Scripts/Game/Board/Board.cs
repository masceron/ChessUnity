using Game.Board.Action;
using Game.Board.Action.Skills;
using Game.Board.Assets;
using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.PieceLogic;
using Game.Board.PieceLogic.Commanders;
using Game.Board.Tile;
using UnityEngine;
using Color = Game.Board.General.Color;

namespace Game.Board
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Board : MonoBehaviour
    {
        [SerializeField] private Piece.PieceManager pieceManager;
        [SerializeField] private TileManager tileManager;
        [SerializeField] public AssetManager assetManager;

        private void MatchMaker()
        {
            MatchManager.Init(tileManager, pieceManager, assetManager, new Config());
            InteractionManager.Init();
            ActionManager.Init();
        }
    
        private void Awake()
        {
            MatchMaker();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MatchManager.GameState.OurSide = MatchManager.GameState.OurSide == Color.White ? Color.Black : Color.White;
                //ActionManager.TakeAction(new EndTurn());
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (InteractionManager.SelectingPiece == -1) return;
                switch (MatchManager.GameState.MainBoard[InteractionManager.SelectingPiece])
                {
                    case Velkaris:
                    {
                        var des = InteractionManager.ActionToTake.Find(action => action.GetType() == typeof(VelkarisKill));
                        if (des != null)
                        {
                            ActionManager.TakeAction(des);
                        }

                        break;
                    }
                    case GuidingSiren when InteractionManager.SelectPieceLock == null:
                    {
                        InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
                        InteractionManager.MarkPiece(InteractionManager.SelectingPiece, typeof(SirenActive));
                        break;
                    }
                    case GuidingSiren:
                        InteractionManager.SelectPieceLock = null;
                        InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
                        break;
                }
            }
            
        }
    }
}
