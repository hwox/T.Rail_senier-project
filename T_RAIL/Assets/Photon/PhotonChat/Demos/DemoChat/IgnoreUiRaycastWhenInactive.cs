using UnityEngine;


public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
{
    public void Start()
    {
        DontDestroyOnLoad(this);
    }
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return gameObject.activeInHierarchy;
    }
}