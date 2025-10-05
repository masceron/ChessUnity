using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic;
using TMPro;
using UnityEngine;
using PrimeTween;
using Game.Effects.Traits;
using Game.Effects.Buffs;
using Game.Action;
using Game.Action.Internal;
using UX.UI.Ingame.DeathDefianceUI;

namespace UX.UI.Ingame.DeathDefianceUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class DeathDefianceUI: MonoBehaviour
    {
        [SerializeField] private GameObject selector;
        [SerializeField] private GameObject EffectItem;
        // [SerializeField] private TMP_Text titleText;

        private int piecePos;
        private List<EffectName> selectedEffects = new();
        private static readonly System.Random random = new System.Random();
        
        private readonly EffectName[] possibleEffects = {
            EffectName.Carapace,
            EffectName.Surpass,
            EffectName.HardenedShield,
            EffectName.Piercing,
            EffectName.Ambush,
            EffectName.Shield

        };
        
        public void Load(int piecePos)
        {
            this.piecePos = piecePos;
            var piece = BoardUtils.PieceOn(piecePos);
            getRandomEffect();
            CreateEffectItems();
        }

        public void getRandomEffect()
        {
            selectedEffects = possibleEffects.OrderBy(x => random.Next()).Take(3).ToList();
        }
        private void CreateEffectItems()
        {
            for (var i = 0; i < 3; i++)
            {
                Instantiate(EffectItem, selector.transform, true);
            }
            for (var i = 0; i < selector.transform.childCount; i++)
            {
                selector.transform.GetChild(i).GetComponent<DeathDefianceItem>().Load(selectedEffects[i]);
            }
        }
        private void DestroyEffectItems()
        {
            for (var i = 0; i < selector.transform.childCount; i++)
            {
                Destroy(selector.transform.GetChild(i).gameObject);
            }
        }
        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            Tween.UIAnchoredPosition(rect, Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        private Effect CreateEffect(EffectName effectName, PieceLogic piece)
        {

            return effectName switch
            {

                EffectName.Carapace => new Game.Effects.Buffs.Carapace(2, piece),
                EffectName.Surpass => new Game.Effects.Traits.Surpass(piece),
                EffectName.HardenedShield => new Game.Effects.Buffs.HardenedShield(piece),
                EffectName.Piercing => new Game.Effects.Buffs.Piercing(2, piece),
                EffectName.Ambush => new Game.Effects.Traits.Ambush(piece),
                _ => new Game.Effects.Buffs.Shield(piece)
            };
        }
        public void ChooseEffect(EffectName effectName)
        {
            var piece = BoardUtils.PieceOn(piecePos);
            var effect = CreateEffect(effectName, piece);
            ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            Disable();
            DestroyEffectItems();
        }
    }
}