using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Followers: Game.Common.Singleton<Followers>
    {
        
        [SerializeField] private Armies armies;
        [SerializeField] private TroopList troopList;
        [SerializeField] private RelicList relicList;
        [SerializeField] private Toggle pieceButton;
        [SerializeField] private Toggle relicButton;

        private void OnDisable()
        {
            troopList.Close();
            relicList.Close();
        }

        private void Awake()
        {
            pieceButton.onValueChanged.AddListener(delegate
            {
                if (pieceButton.isOn)
                {
                    var color = pieceButton.GetComponent<Image>().color;
                    color.a = 1f;
                    pieceButton.GetComponent<Image>().color = color;
                    
                    ChooseTroops();
                }
                else
                {
                    var color = pieceButton.GetComponent<Image>().color;
                    color.a = 0.2f;
                    pieceButton.GetComponent<Image>().color = color;
                }
            });
            
            relicButton.onValueChanged.AddListener(delegate
            {
                if (relicButton.isOn)
                {
                    var color = relicButton.GetComponent<Image>().color;
                    color.a = 1f;
                    relicButton.GetComponent<Image>().color = color;
                    
                    ChooseRelics();
                }
                else
                {
                    var color = relicButton.GetComponent<Image>().color;
                    color.a = 0.2f;
                    relicButton.GetComponent<Image>().color = color;
                }
            });
        }

        private void ChooseTroops()
        {
            relicList.gameObject.SetActive(false);
            troopList.gameObject.SetActive(true);
        }

        private void ChooseRelics()
        {
            troopList.gameObject.SetActive(false);
            relicList.gameObject.SetActive(true);
        }

        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.PlayMenu);
        }

        public void CreateArmy()
        {
            UIManager.Ins.Load(CanvasID.CreateArmy);
        }
    }
}