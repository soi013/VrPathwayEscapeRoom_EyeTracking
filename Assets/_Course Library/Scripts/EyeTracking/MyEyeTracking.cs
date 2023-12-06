using Unity.XR.PXR;
using UnityEngine;

public class MyEyeTracking : MonoBehaviour
{
    private float timeleft;

    void Start()
    {
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

        //Debug.Log($"MyEyeTracking {new { eyeData, eyeData.eyeDatas }}");
        //Debug.Log($"MyEyeTracking {new { eyeData }}");
        //Debug.Log($"MyEyeTracking {new { eyeData.eyeDatas.Length }}");
        //Debug.Log($"MyEyeTracking");
        //Debug.Log($"MyEyeTracking 1 {new { eyeData.eyeDatas[0].pose, eyeData.eyeDatas[0].openness, eyeData.eyeDatas.Length }}");
        PxrVector4f eyeOri = eyeData.eyeDatas[0].pose.orientation;
        Debug.Log($"MyLog 0 {new { eyeOri.w, eyeOri.x, eyeOri.y, eyeOri.z }}");

        eyeOri = eyeData.eyeDatas[1].pose.orientation;
        Debug.Log($"MyLog 1 {new { eyeOri.w, eyeOri.x, eyeOri.y, eyeOri.z }}");

        eyeOri = eyeData.eyeDatas[2].pose.orientation;
        Debug.Log($"MyLog 2 {new { eyeOri.w, eyeOri.x, eyeOri.y, eyeOri.z }}");

        string te = ToDebugText(eyeOri);
    }

    private string ToDebugText(PxrVector4f eyeOri) => $"{new { eyeOri.w, eyeOri.x, eyeOri.y, eyeOri.z }}";

}
