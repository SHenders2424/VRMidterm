using UnityEngine;

public class LookDownMenu : MonoBehaviour
{
    public Camera gazeCamera;
    public GameObject menu;

    [Header("Look Angles (degrees below horizon)")]
    public float showAngle = 30f;
    public float hideAngle = 15f;

    [Header("Placement")]
    public bool repositionOnShow = true;
    public float menuDistance = 2f;
    public float menuPitch = 40f;

    private bool isShowing;

    void Start()
    {
        if (gazeCamera == null)
        {
            gazeCamera = Camera.main;
        }

        menu.SetActive(false);
    }

    void Update()
    {
        float lookDownAngle = -Mathf.Asin(gazeCamera.transform.forward.y) * Mathf.Rad2Deg;

        if (!isShowing && lookDownAngle > showAngle)
        {
            ShowMenu();
        }
        else if (isShowing && lookDownAngle < hideAngle)
        {
            HideMenu();
        }
    }

    void ShowMenu()
    {
        if (repositionOnShow)
        {
            Transform cam = gazeCamera.transform;
            Vector3 flatForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;

            if (flatForward.sqrMagnitude < 0.001f)
            {
                flatForward = Vector3.ProjectOnPlane(cam.up, Vector3.up).normalized;
            }

            Vector3 direction = Quaternion.AngleAxis(menuPitch, Vector3.Cross(Vector3.up, flatForward)) * flatForward;

            menu.transform.position = cam.position + direction * menuDistance;
            menu.transform.rotation = Quaternion.LookRotation(menu.transform.position - cam.position);
        }

        menu.SetActive(true);
        isShowing = true;
    }

    void HideMenu()
    {
        menu.SetActive(false);
        isShowing = false;
    }
}
