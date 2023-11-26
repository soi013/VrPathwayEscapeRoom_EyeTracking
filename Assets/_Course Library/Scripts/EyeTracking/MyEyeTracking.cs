using Unity.XR.PXR;
using UnityEngine;

public class MyEyeTracking : MonoBehaviour
{
    private float timeleft;

    void Start()
    {
        var trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
        Debug.Log($"{new { trackingState }} before start eyeTracking");

        var info = new EyeTrackingStartInfo()
        {
            needCalibration = 1,
            mode = EyeTrackingMode.PXR_ETM_BOTH
        };

        trackingState = (TrackingStateCode)PXR_MotionTracking.StartEyeTracking(ref info);
        Debug.Log($"{new { trackingState }} after start eyeTracking");
    }

    void Update()
    {
        timeleft -= Time.deltaTime;

        if (timeleft >= 0f)
            return;

        timeleft = 1f;

        var state = new EyeTrackingState();
        bool isTracking = false;
        var trackingState = (TrackingStateCode)PXR_MotionTracking.GetEyeTrackingState(ref isTracking, ref state);

        var info = new EyeTrackingDataGetInfo()
        {
            displayTime = 0,
            flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
        | EyeTrackingDataGetFlags.PXR_EYE_POSITION
        | EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION
        };

        var eyeData = new EyeTrackingData();

        trackingState = (TrackingStateCode)PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeData);

        Debug.Log($"{new { eyeData, eyeData.eyeDatas }}");

    }
}
