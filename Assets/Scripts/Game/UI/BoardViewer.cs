using System;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using TMPro;
using UI.UIObject3D.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using static Game.Interaction.BoardInteractionUtils;
using static Game.Board.General.MatchManager;

namespace Game.UI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup pieceAction;
        [SerializeField] private CanvasGroup gameAction;
        
        [SerializeField] private Toggle move;
        [SerializeField] private Toggle capture;
        [SerializeField] private Toggle skill;
        [SerializeField] private TMP_Text skillCoolDown;
        
        [SerializeField] private Button special;
        [SerializeField] private Button relic;
        [SerializeField] private Button skip;

        [SerializeField] private GameObject pieceInfo;
        [SerializeField] private UIObject3D pieceImage;
        [SerializeField] private GameObject pieceStatusEffect;
        [SerializeField] private RawImage pieceDemonstration;

        [NonSerialized] public int SelectingFunction;

        [SerializeField] private GameObject effectUI;

        [SerializeField] public GameObject chrysosShopUI;
        
        private static ColorBlock _normalColors;
        private static ColorBlock _activeColors;

        private bool seeingCapture;
        private PieceLogic hovering;
        
        public void Start()
        {
            _normalColors = move.colors;

            _activeColors = _normalColors;
            _activeColors.normalColor = Color.white;
            _activeColors.selectedColor = Color.white;
            _activeColors.highlightedColor = Color.white;
            _activeColors.pressedColor = Color.white;
            
            move.onValueChanged.AddListener(delegate
            {
                SetToggle(move);
            });
            
            capture.onValueChanged.AddListener(delegate
            {
                SetToggle(capture);
            });
            
            skill.onValueChanged.AddListener(delegate
            {
                SetToggle(skill);
            });
            
            DisablePieceInteractions();
            pieceInfo.SetActive(false);
        }

        private static void SetToggle(Toggle toggle)
        {
            toggle.colors = !toggle.isOn ? _normalColors : _activeColors;
        }

        private static void Disable(Toggle toggle)
        {
            toggle.colors = _normalColors;
            toggle.isOn = false;
        }

        public void DisablePieceInteractions()
        {
            pieceAction.interactable = false;
            Disable(move);
            Disable(capture);
            Disable(skill);
            skillCoolDown.text = "";
            
            SelectingFunction = 0;
        }

        public void EnablePieceInteractions()
        {
            pieceAction.interactable = true;
            var cooldown = gameState.MainBoard[Selecting].SkillCooldown;
            if (cooldown != 0)
            {
                skillCoolDown.text = cooldown > 0 ? cooldown.ToString() : "";
                skill.interactable = false;
            }
            else
            {
                skillCoolDown.text = "";
                skill.interactable = true;
            }
        }

        public void DisableGameInteractions()
        {
            gameAction.interactable = false;
        }

        public void EnableGameInteractions()
        {
            gameAction.interactable = true;
        }

        public void PressMove(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            move.isOn = !move.isOn;

        }

        public void PressCapture(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            capture.isOn = !capture.isOn;
        }

        public void PressSkill(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceAction.interactable) return;
            skill.isOn = !skill.isOn;
        }

        public void ClickMove()
        {
            if (SelectingFunction == 1)
            {
                tileManager.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 1;
            if (MarkMove()) return;
            SelectingFunction = 0;
            move.isOn = false;
        }

        public void ClickCapture()
        {
            if (SelectingFunction == 2)
            {
                tileManager.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 2;
            if (MarkCapture()) return;
            SelectingFunction = 0;
            capture.isOn = false;
        }

        public void ClickSkill()
        {
            if (SelectingFunction == 3)
            {
                tileManager.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 3;
            if (MarkSkill()) return;
            SelectingFunction = 0;
            skill.isOn = false;
        }

        public void PressEndTurn(InputAction.CallbackContext context)
        {
            if (!context.performed || !skip.interactable) return;
            EndTurn();
        }
        
        public void EndTurn()
        {
            Unmark();
            NewTurn();
        }

        public void SetPieceHover(int pos)
        {
            if (Selecting != -1) return;
            
            if (pos == -1)
            {
                hovering = null;
                pieceStatusEffect.SetActive(false);
                pieceInfo.SetActive(false);
                return;
            }
            
            var piece = gameState.MainBoard[pos];

            if (piece == null) return;

            hovering = piece;
            SetUpPieceInfo(piece);
            SetUpStatusEffects(piece);
        }

        private void SetUpStatusEffects(PieceLogic piece)
        {
            pieceStatusEffect.SetActive(true);
            var already = pieceStatusEffect.transform.childCount;
            var needed = piece.Effects.Count;
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(effectUI, pieceStatusEffect.transform, true);
                }
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(pieceStatusEffect.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            for (var i = 0; i < needed; i++)
            {
                pieceStatusEffect.transform.GetChild(i).GetComponent<EffectUI>().Set(piece.Effects[i]);
            }
        }
        
        private void SetUpPieceInfo(PieceLogic piece)
        {
            pieceInfo.SetActive(true);
            var pieceInformation = assetManager.PieceData[piece.type];
            LoadPieceModel(pieceInformation);
            LoadPieceDemonstrations(pieceInformation);
        }
        
        private void LoadPieceModel(PieceObject info)
        {
            pieceImage.ObjectPrefab = info.prefab.transform;
        }
        
        private void LoadPieceDemonstrations(PieceObject info)
        {
            if (!seeingCapture)
            {
                if (info.movePattern)
                {
                    pieceDemonstration.texture = info.movePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            else
            {
                if (info.capturePattern)
                {
                    pieceDemonstration.texture = info.capturePattern;
                    pieceDemonstration.color = Color.white;
                    return;
                }
            }
            
            pieceDemonstration.texture = null;
            pieceDemonstration.color = new Color(0, 0, 0, 0);
        }

        public void ToggleDemonstrations(InputAction.CallbackContext context)
        {
            if (!context.performed || !pieceInfo.activeSelf) return;
            
            seeingCapture = !seeingCapture;
            LoadPieceDemonstrations(assetManager.PieceData[hovering.type]);
        }
    }
}
