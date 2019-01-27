using System.Collections;
using System.Collections.Generic;
using ElMoro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneCoordinator : MonoBehaviour
{
    public bool displayStartWindowOnStart;
    public WindowSchematic startWindow;
    public WindowSchematic finishWindow;
    public WindowSchematic trackingWindow;

    private GameObject startWindowInstance;
    private GameObject finishWindowInstance;

    [Inject]
    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (displayStartWindowOnStart)
        {
            startWindowInstance = WindowCreator.instance.CreateWindow(startWindow);
            startWindowInstance.GetComponentInChildren<StartMenuLogic>().Populate(StartGame, QuitGame);
        }
    }

    /// <summary>
    /// event for loading the right scene for starting the game
    /// </summary>
    public void StartGame()
    {
        startWindowInstance.GetComponentInChildren<WindowContainer>()
            .CloseWindow();
        gameManager.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
