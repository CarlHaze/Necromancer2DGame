using UnityEngine;
using UnityEngine.UIElements;


public class MainMenuEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");    

        });


    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");

        });
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
