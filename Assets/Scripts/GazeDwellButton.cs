using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GazeDwellButton : MonoBehaviour
{
    public float gazeDistance = 10f;
    public float dwellTime = 3f;

    public LayerMask buttonLayer;
    public Camera gazeCamera;

    private Button currentButton;
    private Coroutine dwellCoroutine;

   // public AudioSource gazeSound;


    void Update()
    {
        // Create gaze ray
        Ray ray = new Ray(
            gazeCamera.transform.position,
            gazeCamera.transform.forward
        );

        Debug.DrawRay(ray.origin, ray.direction * gazeDistance,
            Color.red
        );

        // Is the user looking at a button?
        if (Physics.Raycast(
            ray, out RaycastHit hit, gazeDistance, buttonLayer))
        {
            Button newButton = hit.collider.GetComponent<Button>();

            // Started looking at a new button
            if (newButton != currentButton)
            {
                ResetButton();

                currentButton = newButton;

                if (currentButton != null)
                {
                    // Highlight
                    currentButton.image.color =
                        currentButton.colors.highlightedColor;

                    // Start dwell Coroutine
                    dwellCoroutine =
                        StartCoroutine(DwellAndActivate());

                   // gazeSound.Play();
                }
            }
        }
        else
        {
            // User looked away
            ResetButton();
        }
    }

    IEnumerator DwellAndActivate()
    {
        // Dwell
        yield return new WaitForSeconds(dwellTime);

        // Pressed
        currentButton.image.color = currentButton.colors.pressedColor;

        yield return new WaitForSeconds(0.2f);

        // Execute Inspector OnClick()
        currentButton.onClick.Invoke();

        // Stay highlighted
        currentButton.image.color = currentButton.colors.highlightedColor;

        dwellCoroutine = null;
    }

    void ResetButton()
    {
        // Cancel dwell
        if (dwellCoroutine != null)
        {
            StopCoroutine(dwellCoroutine);
            dwellCoroutine = null;
        }

        // Return button to normal
        if (currentButton != null)
        {
            currentButton.image.color = currentButton.colors.normalColor;
        }

        currentButton = null;

       // gazeSound.Stop();
    }
}
