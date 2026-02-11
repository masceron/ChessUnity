using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UX.UI.Tooltip;
using ZLinq;
using Color = UnityEngine.Color;
using static Game.Common.BoardUtils;
using static UX.UI.Ingame.BoardViewer;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceActions: MonoBehaviour
    {
        [SerializeField] private CanvasGroup pieceAction;
        [SerializeField] private Toggle move;
        [SerializeField] private Toggle capture;
        [SerializeField] private Toggle skill;
        [SerializeField] private TooltipTrigger skillTooltip;
        [SerializeField] private TMP_Text skillCoolDown;
        
        private static ColorBlock _normalColors;
        private static ColorBlock _activeColors;

        private List<Action> _listOf;
        private List<Action> _moveList;

        public void Load(List<Action> l, List<Action> ml)
        {
            _listOf = l;
            _moveList = ml;
        }

        private void Start()
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
        }

        public void EnablePieceInteractions()
        {
            pieceAction.interactable = true;
            skill.interactable = PieceOn(Selecting).SkillCooldown == 0;
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
                TileManager.Ins.UnmarkAll();
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
                TileManager.Ins.UnmarkAll();
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
                TileManager.Ins.UnmarkAll();
                SelectingFunction = 0;
                return;
            }
            
            SelectingFunction = 3;
            if (MarkSkill()) return;
            SelectingFunction = 0;
            skill.isOn = false;
        }
        
        private void LoadSkillInfo()
        {
            var info = AssetManager.Ins.PieceData[PieceOn(Selecting).Type];
            if (info.hasSkill)
            {
                skillTooltip.enabled = true;
                skillTooltip.SetText("Skill: " + Localizer.GetText("piece_skill", info.key + "_skill", null), "",
                    Localizer.GetText("piece_skill_description", info.key + "_skill_description", null));
            }
            else
            {
                skillTooltip.enabled = false;
            }
        }

        public void LoadPieceActionInfo()
        {
            var cooldown = PieceOn(Selecting).SkillCooldown;
            LoadSkillInfo();
            if (cooldown != 0)
            {
                skillCoolDown.text = cooldown > 0 ? cooldown.ToString() : "";
            }
            else
            {
                skillCoolDown.text = "";
            }
        }
        
        private bool MarkMove()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var quiets in _moveList.OfType<IQuiets>())
            {
                _listOf.Add((Action)quiets);
                TileManager.Ins.MarkAsMoveable(((Action)quiets).Target);
            }
            
            return _listOf.Count > 0;
        }

        private bool MarkCapture()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var captures in _moveList.OfType<ICaptures>())
            {
                var targetPos = ((Action)captures).Target;
                if (FormationManager.Ins.IsHideByFog(targetPos, SideToMove())){ continue; }
                _listOf.Add((Action)captures);
                TileManager.Ins.MarkAsMoveable(((Action)captures).Target);
            }

            return _listOf.Count > 0;
        }

        private bool MarkSkill()
        {
            TileManager.Ins.UnmarkAll();
            _listOf.Clear();
            
            foreach (var skills in _moveList.OfType<ISkills>())
            {
                var targetPos = ((Action)skills).Target;
                if (FormationManager.Ins.IsHideByFog(targetPos, SideToMove())){ continue; }
                _listOf.Add((Action)skills);
                TileManager.Ins.MarkAsMoveable(((Action)skills).Target);
            }
            
            return _listOf.Count > 0;
        }
    }
}