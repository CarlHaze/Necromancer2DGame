using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace NecromancersRising.UI
{
    public class MainMenuEvents : MonoBehaviour
    {
        [SerializeField] private string gameSceneName = "SampleScene";
        [SerializeField] private float sceneTransitionDelay = 0.5f;
        
        private UIDocument document;
        private List<Button> menuButtons = new List<Button>();
        private AudioSource audioSource;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            document = GetComponent<UIDocument>();
            
            // Query all buttons and register callback
            menuButtons = document.rootVisualElement.Query<Button>().ToList();
            foreach (Button button in menuButtons)
            {
                button.RegisterCallback<ClickEvent>(OnButtonClicked);
            }
        }
        
        private void OnDisable()
        {
            // Clean up callbacks
            foreach (Button button in menuButtons)
            {
                button.UnregisterCallback<ClickEvent>(OnButtonClicked);
            }
        }
        
        private void OnButtonClicked(ClickEvent ev)
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
            
            Invoke(nameof(LoadGameScene), sceneTransitionDelay);
        }
        
        private void LoadGameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}