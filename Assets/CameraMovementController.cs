using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public static CameraMovementController instance;
    private Animator anim;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            ShakeTheCamera(0.3f);
        }
    }

    public async void ShakeTheCamera(float seconds)
    {
        anim.SetBool("shake", true);

        await Task.Delay((int)(seconds * 1000));

        anim.SetBool("shake", false);

    }
}
