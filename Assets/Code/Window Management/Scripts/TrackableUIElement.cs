using UnityEngine;

public class TrackableUIElement : MonoBehaviour
{
    public GameObject objectToTrack;
    public Vector3 deltaPosition;
    public RectTransform contents;
    public bool trackOnStart;

    bool isTracking = false;
    private RectTransform parentRect;

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
                Vector3 destination = SetInterfacePosition() + deltaPosition;
                contents.anchoredPosition = destination;
                //Debug.Log($"Moving {this.gameObject.name}'s contents to {destination}");
            }
        }
    }

    /// <summary>
    /// manages setting the position of a trackable UI element
    /// </summary>
    /// <returns>The interface position.</returns>
    private Vector3 SetInterfacePosition()
    {
        // get the screen point of the game object
        if (objectToTrack != null)
            return Camera.main.WorldToScreenPoint(objectToTrack.transform.position);
        else
            return Vector3.zero;

    }

    public void ToggleTracking(bool active)
    {
        isTracking = active;
        if(!active)
            contents.anchoredPosition = new Vector3(0, 5000, 0); // just get rid of the object
    }
}
