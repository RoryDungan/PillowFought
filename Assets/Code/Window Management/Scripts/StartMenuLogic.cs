using ElMoro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class StartMenuLogic : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    private UnityAction playEvent;
    private UnityAction exitEvent;

    [SerializeField]
    private WindowSchematic scoreKeeperScheme;
    private GameObject scoreKeeperInstance;

    /// <summary>
    /// set up the events for this function
    /// </summary>
    /// <param name="play">Play.</param>
    /// <param name="exit">Exit.</param>
    public void Populate(UnityAction play, UnityAction exit)
    {
        playButton.onClick.AddListener(play);
        exitButton.onClick.AddListener(exit);

        playEvent = play;
        exitEvent = exit;
    }

    private void Update()
    {
        if (Input.GetButtonDown(InputManager.Player0Throw)
            || Input.GetButtonDown(InputManager.Player1Throw)
            || Input.GetKeyDown(KeyCode.F))
        {
            scoreKeeperInstance = WindowCreator.instance.CreateWindow(scoreKeeperScheme);
            playEvent.Invoke();
        }
        if (Input.GetButtonDown(InputManager.Player0Grab)
            || Input.GetButtonDown(InputManager.Player1Grab)
            || Input.GetKeyDown(KeyCode.Space))
        {
            exitEvent.Invoke();
        }
    }

}
