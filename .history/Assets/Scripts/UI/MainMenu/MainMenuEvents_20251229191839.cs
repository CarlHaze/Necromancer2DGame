using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MainMenuEvents : MonoBehaviour
{
    private UIDocument document;

    private Button button;

    //list of buttons so dont need to set them up individually
    private List<Button> menuButtons = new List<Button>();



    private void Awake()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");    

        });

        menuButtons = document.rootVisualElement.Query<Button>().ToList();
        //call back for each button
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonClicked);
        }


    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(ev =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

        });
    }

    private void OnAllButtonClicked(ClickEvent ev)
    {
        Debug.Log("A button was clicked!");
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
