using UnityEngine;
using UnityEngine.UI;

public class TrackableUIElement : MonoBehaviour
{
    public GameObject objectToTrack;
    public Vector3 deltaPosition;
    public RectTransform contents;
    public bool trackOnStart;

    bool isTracking = false;
    private RectTransform parentRect;

    private Camera mainCamera;
    private Vector2 referenceResolution;

    private void Awake()
    {
        mainCamera = Camera.main;
        var canvasScaler = GetComponentInParent<CanvasScaler>();
        referenceResolution = canvasScaler.referenceResolution;
    }

    public void StartTracking(GameObject trackingObject)
    {
        parentRect = GetComponentInParent<RectTransform>();
        isTracking = trackOnStart;
        Debug.Log($"Staring Tracking {trackingObject.name}");
        objectToTrack = trackingObject;
    }

    private void ShowTracking()
    {
        isTracking = true;
    }

    private void Update()
    {
        if (isTracking)
        {
            if (objectToTrack != null)
            {
                Vector3 destination = GetInterfacePosition() + deltaPosition;
                contents.anchoredPosition = destination;
                //Debug.Log($"Moving {this.gameObject.name}'s contents to {destination}");
            }
        }
    }

    /// <summary>
    /// manages setting the position of a trackable UI element
    /// </summary>
    /// <returns>The interface position.</returns>
    private Vector3 GetInterfacePosition()
    {
        // get the screen point of the game object
        if (objectToTrack != null)
        {
            var pos = mainCamera.WorldToScreenPoint(objectToTrack.transform.position);
            return new Vector2(
                (pos.x / mainCamera.pixelWidth) * referenceResolution.x,
                (pos.y / mainCamera.pixelHeight) * referenceResolution.y
            );
        }
        else
        {
            return Vector3.zero;
        }

    }

    public void ToggleTracking(bool active)
    {
        isTracking = active;
        if(!active)
            contents.anchoredPosition = new Vector3(0, 5000, 0); // just get rid of the object
    }
}
