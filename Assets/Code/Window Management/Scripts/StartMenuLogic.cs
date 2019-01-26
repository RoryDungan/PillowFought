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

    [Inject]
    private IInputManager inputManager;

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
        if (inputManager.GetGrabButtonDown(0))
        {
            playEvent.Invoke();
        }
        else if (inputManager.GetThrowButtonDown(0))
        {
            exitEvent.Invoke();
        }
    }

}
