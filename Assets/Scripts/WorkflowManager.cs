using UnityEngine;

namespace WorkflowSystem
{
    public class WorkflowManager : MonoBehaviour
    {
        public static WorkflowManager Instance { get; private set; }

        private Workflow currentWorkflow;
        private int currentStepIndex = 0;

        // Define the "end" string as a constant
        private const string EndStepId = "end";

        public Workflow CurrentWorkflow => currentWorkflow;
        public int CurrentStepIndex => currentStepIndex;

        private IJsonLoader jsonLoader;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Initialize the JSON loader (local for now)
            jsonLoader = new LocalJsonLoader("workflow.json");

            // Load JSON data
            LoadJsonData();
        }

        void LoadJsonData()
        {
            string json = jsonLoader.LoadJson();
            if (!string.IsNullOrEmpty(json))
            {
                currentWorkflow = JsonUtility.FromJson<Workflow>(json);
                UIManager.Instance.DisplayStep(currentWorkflow.stepData[currentStepIndex]);
            }
            else
            {
                Debug.LogError("Failed to load JSON data.");
            }
        }

        public void NavigateToStep(string nextStepId)
        {
            // Use the constant variable instead of hardcoding "end"
            if (nextStepId == EndStepId)
            {
                QuitWorkflow();
                return;
            }

            for (int i = 0; i < currentWorkflow.stepData.Length; i++)
            {
                if (currentWorkflow.stepData[i].stepId == nextStepId)
                {
                    currentStepIndex = i;
                    UIManager.Instance.DisplayStep(currentWorkflow.stepData[currentStepIndex]);
                    break;
                }
            }
        }

        private void QuitWorkflow()
        {
            Debug.Log("Workflow Ended");
        }
    }

    [System.Serializable]
    public class Workflow
    {
        public string workflowId;
        public string workflowName;
        public string createdBy;
        public string created;
        public Toggles toggles;
        public StepData[] stepData;
    }

    [System.Serializable]
    public class Toggles
    {
        public bool callSupport;
        public bool documentation;
        public bool pauseTask;
        public bool cancelTask;
    }

    [System.Serializable]
    public class StepData
    {
        public string stepId;
        public string stepName;
        public int uiTemplate;
        public string imageUrl;
        public int imageScaleType;
        public string title;
        public string text;
        public ButtonData[] buttons;
    }

    [System.Serializable]
    public class ButtonData
    {
        public int id;
        public string text;
        public string nextStepId;
    }
}
