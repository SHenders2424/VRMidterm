using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GazeDwellButton : MonoBehaviour
{
    public float gazeDistance = 10f;
    public float dwellTime = 3f;

    public LayerMask buttonLayer;
    public Camera gazeCamera;

    [Header("UI Feedback")]
    public Image radialProgressBar; // Assign a UI Image here (Image Type must be set to Filled)

    private Button currentButton;
    private Coroutine dwellCoroutine;

    // public AudioSource gazeSound;

    void Start()
    {
        // Ensure the progress bar starts empty
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 0f;
        }
    }

    void Update()
    {
        // Create gaze ray
        Ray ray = new Ray(
            gazeCamera.transform.position,
            gazeCamera.transform.forward
        );

        Debug.DrawRay(ray.origin, ray.direction * gazeDistance, Color.red);

        // Is the user looking at a button?
        if (Physics.Raycast(ray, out RaycastHit hit, gazeDistance, buttonLayer))
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
                    currentButton.image.color = currentButton.colors.highlightedColor;

                    // Start dwell Coroutine
                    dwellCoroutine = StartCoroutine(DwellAndActivate());

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
        float elapsedTime = 0f;

        // Dynamic dwell loop to update the progress bar
        while (elapsedTime < dwellTime)
        {
            elapsedTime += Time.deltaTime;

            if (radialProgressBar != null)
            {
                // Map progress to a 0.0 - 1.0 range
                radialProgressBar.fillAmount = elapsedTime / dwellTime;
            }

            yield return null; // Wait for the next frame
        }

        // Ensure it visually hits 100%
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 1f;
        }

        // Pressed
        currentButton.image.color = currentButton.colors.pressedColor;

        yield return new WaitForSeconds(0.2f);

        // Execute Inspector OnClick()
        currentButton.onClick.Invoke();

        // Stay highlighted
        currentButton.image.color = currentButton.colors.highlightedColor;

        // Clear progress bar after activation
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 0f;
        }

        dwellCoroutine = null;
    }

    void ResetButton()
    {
        if (currentButton != null)
        {
            // Reset to normal color
            currentButton.image.color = currentButton.colors.normalColor;
            currentButton = null;
        }

        if (dwellCoroutine != null)
        {
            StopCoroutine(dwellCoroutine);
            dwellCoroutine = null;
        }

        // Reset progress bar back to 0 if the user looks away
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 0f;
        }
    }
}
