using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Ingame
{
    public class TopInfoBox: MonoBehaviour
    {
        private BoardViewer _boardViewer;
        private Button _surrender;
        private Button _callDraw;
        private Label _turnCount;

        private void Awake()
        {
            _boardViewer = GetComponent<BoardViewer>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _surrender = root.Q<Button>("Surrender");
            _callDraw = root.Q<Button>("CallDraw");

            _turnCount = root.Q<Label>("RoundCounter");
            _turnCount.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath("CurrentTurn"),
                bindingMode = BindingMode.ToTarget
            });
        }
    }
}