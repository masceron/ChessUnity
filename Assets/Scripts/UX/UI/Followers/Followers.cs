using Game.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Followers : Singleton<Followers>
    {
        [SerializeField] private SavedArmies savedArmies;
        [SerializeField] private TroopList troopList;
        [SerializeField] private RelicList relicList;
        [SerializeField] private Toggle pieceButton;
        [SerializeField] private Toggle relicButton;

        protected override void Awake()
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

        private void OnEnable()
        {
            savedArmies.Load();
        }

        private void OnDisable()
        {
            troopList.Close();
            relicList.Close();
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
            UIManager.Ins.Load(CanvasID.StartGame);
        }

        public void CreateArmy()
        {
            UIManager.Ins.Load(CanvasID.CreateArmy);
        }

        public void OnClickPrevious()
        {
            UIManager.Ins.LoadPreviousCanvas();
        }
    }
}