using UnityEngine;

[DisallowMultipleComponent]
public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    public static CameraManager Instance;

    public Camera Cam
    {
        get { return cam ; }
        set { cam = value; }
    }

    void Awake()
    {
        cam = Camera.main;
        #region Singleton
        if (Instance == null) { Instance = this; }
        #endregion
    }

    public Vector3 GetScreenBounds() //performance issues by calling multiple times Camera.main
    {
        Vector3 camPos = cam.transform.position;
        return cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camPos.z)) - camPos;
    }

    //public bool IsVisibleToCamera(Transform t)
    //{
    //    Vector3 camPos = cam.transform.position;
    //    Vector3 pos = t.position;
    //    Vector3 screenBounds = GetScreenBounds();

    //    return (Mathf.Abs(camPos.x - pos.x) <= screenBounds.x && Mathf.Abs(camPos.y - pos.y) <= screenBounds.y);
    //}
    
    //https://answers.unity.com/questions/8003/how-can-i-know-if-a-gameobject-is-seen-by-a-partic.html

    public bool IsTargetVisible(GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }
}
