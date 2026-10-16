using UnityEngine;

public class CursorPositioner : MonoBehaviour
{
    private Transform cameraTransform;



    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        float targetDistance = 0;

        if (Physics.Raycast(ray, out hit))
        {

            targetDistance = hit.distance;
        }

        transform.localPosition = new Vector3(0, 0, hit.distance);

        float calculatedScale = targetDistance * (float)0.002;
        transform.localScale = new Vector3(calculatedScale, calculatedScale, calculatedScale);
    }
}
