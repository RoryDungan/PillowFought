using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
///  This class manages where UI subcategories are placed and when to destroy itself
/// </summary>
public class WindowContainer : MonoBehaviour
{
    private const string DISMISS_WINDOW = "dismissWindow";
    private const string SHOW_WINDOW = "showWindow";

    [Tooltip("this is where the actual content is placed into when the object is ")]
    public Transform content;

    private Animator _anim;

    /// <summary>
    /// Tells the window to animate in, and to place itself at the right position in the scene
    /// </summary>
    public void ShowThisWindow(int siblingOrder)
    {
        _anim = GetComponent<Animator>();
        _anim.ResetTrigger(DISMISS_WINDOW);
        _anim.SetTrigger(SHOW_WINDOW);
    }

    /// <summary>
    /// this class will close the window and animate it out before destroying it
    /// </summary>
    public async void CloseWindow()
    {
        Animator anim = GetComponent<Animator>();

        if(anim != null)
        {
            anim.SetTrigger(DISMISS_WINDOW);
            anim.ResetTrigger(SHOW_WINDOW);

            // wait half a second for the animation to complete
            await Task.Delay(500);
        }

        Destroy(this.gameObject);  
    }
}






