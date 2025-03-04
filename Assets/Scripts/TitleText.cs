using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace WorkflowSystem
{
    public class TitleText : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI contentText;
        public GameObject buttonPrefab;
        public Transform buttonContainer;

        private List<GameObject> buttonPool = new List<GameObject>();
        private const int INITIAL_BUTTON_POOL_SIZE = 5;

        void Awake()
        {
            InitializeButtonPool();
        }

        private void InitializeButtonPool()
        {
            // Create initial pool of buttons
            for (int i = 0; i < INITIAL_BUTTON_POOL_SIZE; i++)
            {
                CreateButtonForPool();
            }
        }

        private GameObject CreateButtonForPool()
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, 50);
            button.SetActive(false);
            buttonPool.Add(button);
            return button;
        }

        public void UpdateTitleAndText(string title, string text)
        {
            titleText.text = title;
            contentText.text = text;
        }

        public void UpdateButtons(ButtonData[] buttons)
        {
            // Ensure we have enough buttons in the pool
            while (buttonPool.Count < buttons.Length)
            {
                CreateButtonForPool();
            }

            // Deactivate all buttons first
            foreach (GameObject button in buttonPool)
            {
                button.SetActive(false);
            }

            // Activate and configure needed buttons
            for (int i = 0; i < buttons.Length; i++)
            {
                GameObject buttonObj = buttonPool[i];
                buttonObj.SetActive(true);

                // Update button text
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = buttons[i].text;
                }

                // Update click handler
                Button button = buttonObj.GetComponent<Button>();
                string nextStepId = buttons[i].nextStepId; // Capture the value for the closure
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => WorkflowManager.Instance.NavigateToStep(nextStepId));
            }
        }
    }
}