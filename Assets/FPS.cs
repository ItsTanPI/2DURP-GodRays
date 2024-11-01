using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    [SerializeField] Text fpsText;

    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    void Start()
    {
        if (fpsText == null)
        {
            fpsText = GetComponent<Text>();
        }
    }

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        if (deltaTime > 1.0f)
        {
            fps = frameCount / deltaTime;

            fpsText.text = string.Format("{0:0.}", fps);

            frameCount = 0;
            deltaTime -= 1.0f;
        }
    }
}
