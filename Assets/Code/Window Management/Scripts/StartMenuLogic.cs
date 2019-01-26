using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMenuLogic : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    /// <summary>
    /// set up the events for this function
    /// </summary>
    /// <param name="play">Play.</param>
    /// <param name="exit">Exit.</param>
    public void Populate(UnityAction play, UnityAction exit)
    {
        playButton.onClick.AddListener(play);
        exitButton.onClick.AddListener(exit);
    }
}
