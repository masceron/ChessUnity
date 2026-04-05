using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal.Pending;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using static Game.Common.BoardUtils;
using Action = Game.Action.Action;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoardViewer : Singleton<BoardViewer>
    {
        public static PieceLogic Hovering;

        public static int HoveringPos;

        // Vị trí của quân cờ đang chọn, -1 nếu chưa chọn quân nào
        public static int Selecting = -1;

        // 0: Không chọn, 1: Move, 2: Attack, 3: Skill, 4: Relic
        public static int SelectingFunction;
        private static readonly List<Action> MoveList = new();
        public static readonly List<Action> ListOf = new();
        [SerializeField] private PieceInfoMenu pieceInfoMenu;
        [SerializeField] private PieceActions pieceActions;
        [SerializeField] private GameActions gameActions;
        [SerializeField] private EffectBar effectBar;

        private AIManager _aiManager;

        private Transform _mainCameraCenter;

        private void Start()
        {
            _mainCameraCenter = GameObject.Find("CameraTarget").GetComponent<Transform>();
            _aiManager = AIManager.Ins;
            pieceActions.Load(ListOf, MoveList);
            UpdateRelic();
            Selecting = -1;
            SelectingFunction = 0;
        }

        private void OnEnable()
        {
            MatchManager.Ins.InputProcessor = this;
        }

        public void UpdateRelic()
        {
            gameActions.UpdateRelic();
        }

        private void PanTo(int pos1, int pos2)
        {
            var direction = _mainCameraCenter.position;
            direction.x = pos1;
            direction.z = pos2;

            Tween.Position(_mainCameraCenter, direction, 0.3f);
        }

        private void SetPieceHover(int pos)
        {
            HoveringPos = pos;

            if (Selecting != -1) return;

            var piece = PieceOn(pos);

            if (piece is not { IsVisible: true }) return;

            Hovering = piece;

            pieceInfoMenu.SetUpPieceInfo(piece);
            effectBar.SetUpStatusEffects(piece);
        }

        public void QuitMenu(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.QuitToMainMenu);
        }

        public void Unmark()
        {
            Selecting = -1;
            TileManager.Ins.UnmarkAll();
            _aiManager.ClearTileScores();
            pieceActions.DisablePieceInteractions();
            foreach (var action in ListOf)
            {
                if (action is IDisposable disposable) disposable.Dispose();
                if (action is PendingAction pending) pending.CancelResult(); //hủy những task đang treo
            }

            ListOf.Clear();
            Hover(-1);
        }

        public async void ExecuteAction(Action action)
        {
            if (action is PendingAction pendingAction) action = await pendingAction.WaitForCompletion();

            if (ActionManager.DoManualAction(action)) EndTurn();
            Unmark();
        }

        public void MarkPiece(int pos)
        {
            HoveringPos = pos;
            _aiManager.ShowPieceActionScore(pos);

            if (Selecting != -1)
            {
                switch (SelectingFunction)
                {
                    case 0:
                        return;
                    case 4:
                    {
                        var action = ListOf.Find(a => a.GetFrom() == pos);
                        if (action == null) return;
                        ExecuteAction(action);
                        break;
                    }
                    default:
                    {
                        var action = ListOf.Find(a => a.GetTargetPos() == pos);
                        if (action == null) return;
                        ExecuteAction(action);
                        break;
                    }
                }
            }
            else
            {
                // Hiển thị những vị trí trên bàn cờ có thể thực thi Action do người chơi chọn
                // Action ở đây có thể là Move/Attack/Skill
                var piece = PieceOn(pos);
                if (piece is not { IsVisible: true } || FormationManager.IsHideByFog(pos, SideToMove())) return;

                Hover(pos);
                TileManager.Ins.Select(pos);
                Selecting = pos;
                pieceActions.LoadPieceActionInfo();
                PanTo(RankOf(pos), FileOf(pos));

                if (piece.Color != GetSideToMove()) return;

                pieceActions.EnablePieceInteractions();
                MoveList.Clear();
                PieceOn(Selecting).MoveList(MoveList);
            }
        }

        private void EndTurn()
        {
            if (SideToMove() != OurSide())
                gameActions.DisableGameInteractions();
            else
                gameActions.EnableGameInteractions();
        }

        public void Hover(int pos)
        {
            if (Selecting != -1) return;
            HoveringPos = pos;
            if (pos == -1)
            {
                Hovering = null;
                effectBar.Disable();
                pieceInfoMenu.Disable();
                return;
            }

            if (FormationManager.IsHideByFog(pos, SideToMove())) return;

            SetPieceHover(pos);
        }

        public T GetOrInstantiateUI<T>(IngameSubmenus submenuType) where T : Component
        {
            var menu = FindAnyObjectByType<T>(FindObjectsInactive.Include);

            if (!menu)
                menu = Instantiate(UIHolder.Ins.Get(submenuType), transform).GetComponent<T>();
            else
                menu.gameObject.SetActive(true);

            return menu;
        }
    }
}