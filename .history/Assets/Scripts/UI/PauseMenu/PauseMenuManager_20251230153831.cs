using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace NecromancersRising.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        
        private UIDocument document;
        private VisualElement pauseMenuContainer;
        private Button resumeButton;
        private Button quitButton;
        
        private bool isPaused = false;
        
        private void Awake()
        {
            document = GetComponent<UIDocument>();
            
            // MATCH THESE NAMES IN UI BUILDER:
            pauseMenuContainer = document.rootVisualElement.Q<VisualElement>("PauseMenuContainer");
            resumeButton = document.rootVisualElement.Q<Button>("ResumeButton");
            quitButton = document.rootVisualElement.Q<Button>("QuitButton");
            
            // Register button callbacks
            if (resumeButton != null)
                resumeButton.RegisterCallback<ClickEvent>(ev => ResumeGame());
            
            if (quitButton != null)
                quitButton.RegisterCallback<ClickEvent>(ev => QuitToMainMenu());
            
            // Start with menu hidden
            HideMenu();
        }
        
        private void Update()
        {
            // Toggle pause with ESC key
            if (Input.GetKeyDown(pauseKey))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
        
        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f; // Freeze everything
            ShowMenu();
        }
        
        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f; // Resume normal time
            HideMenu();
        }
        
        private void ShowMenu()
        {
            if (pauseMenuContainer != null)
                pauseMenuContainer.style.display = DisplayStyle.Flex;
        }
        
        private void HideMenu()
        {
            if (pauseMenuContainer != null)
                pauseMenuContainer.style.display = DisplayStyle.None;
        }
        
        private void QuitToMainMenu()
        {
            // IMPORTANT: Reset time scale before changing scenes!
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
        
        private void OnDisable()
        {
            // Cleanup callbacks
            if (resumeButton != null)
                resumeButton.UnregisterCallback<ClickEvent>(ev => ResumeGame());
            
            if (quitButton != null)
                quitButton.UnregisterCallback<ClickEvent>(ev => QuitToMainMenu());
        }
    }
}