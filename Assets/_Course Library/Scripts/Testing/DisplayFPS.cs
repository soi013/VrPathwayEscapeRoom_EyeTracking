using System.Collections;
using TMPro;
using Unity.XR.PXR;
using UnityEngine;


public class DisplayFPS : MonoBehaviour
{
    public float updateInteval = 0.5f;

    private TextMeshProUGUI textOutput = null;

    private float deltaTime = 0.0f;
    private float milliseconds = 0.0f;
    private int framesPerSecond = 0;

    private void Awake()
    {
        textOutput = GetComponentInChildren<TextMeshProUGUI>();

        var trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
        Debug.Log($"MyLog {new { trackingState }} before start eyeTracking");

        var info = new EyeTrackingStartInfo()
        {
            needCalibration = 1,
            mode = EyeTrackingMode.PXR_ETM_BOTH
        };

        trackingState = (TrackingStateCode)PXR_MotionTracking.StartEyeTracking(ref info);
        Debug.Log($"MyLog {new { trackingState }} after start eyeTracking");
    }

    private void Start()
    {
        StartCoroutine(ShowFPS());
    }

    private void Update()
    {
        CalculateCurrentFPS();
    }

    private void CalculateCurrentFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        milliseconds = (deltaTime * 1000.0f);
        framesPerSecond = (int)(1.0f / deltaTime);
    }

    private IEnumerator ShowFPS()
    {
        while (true)
        {
            textOutput.color = CalcColorFromFps(framesPerSecond);

            var info = new EyeTrackingDataGetInfo()
            {
                displayTime = 0,
                flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
            | EyeTrackingDataGetFlags.PXR_EYE_POSITION
            | EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION
            };


            var eyeData = new EyeTrackingData();

            var trackingState = (TrackingStateCode)PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeData);

            var eyeOri = eyeData.eyeDatas[2].pose.orientation;
            string eyeOriText = ToDebugText(eyeOri);

            Debug.Log($"MyLog 2 {eyeOriText}");

            var eyePos = eyeData.eyeDatas[2].pose.position;
            string eyePosText = ToDebugText(eyePos);

            textOutput.text = $"FPS:{framesPerSecond}  MS:{milliseconds:.0}\nEOri:{eyeOriText}\nEPos:{eyePosText}";

            //var currentPosition = this.transform.position;

            //this.transform.position = new Vector3(eyePos.y * 100, currentPosition.y, currentPosition.z);
            //this.transform.position = new Vector3(currentPosition.y + 10f, currentPosition.y, currentPosition.z);

            yield return new WaitForSeconds(updateInteval);
        }
    }

    private static Color CalcColorFromFps(int fps)
    {
        const float goodFpsThreshold = 72;
        const float badFpsThreshold = 50;

        if (fps >= goodFpsThreshold)
        {
            return Color.green;
        }
        else if (fps >= badFpsThreshold)
        {
            return Color.yellow;
        }
        else
        {
            return Color.red;
        }

    }

    private string ToDebugText(PxrVector4f eyeOri)
    {
        return $"x={eyeOri.x * 100:F1} y={eyeOri.y * 100:F1} z={eyeOri.z * 100:F1} w={eyeOri.w * 100:F1}";
    }
    private string ToDebugText(PxrVector3f eyePos)
    {
        return $"x={eyePos.x * 100:F1} y={eyePos.y * 100:F1} z={eyePos.z * 100:F1}";
    }
}
