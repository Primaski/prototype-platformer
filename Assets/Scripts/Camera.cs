using UnityEngine;

public class Camera : MonoBehaviour{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    [Range(1,10)]
    public int smoothScrollFactor = 5;

    /* if boundedCamera is set to False, minValues and maxValues will be ignored and the camera will
     * have no boundaries. Otherwise, set min and max values according to the map.*/
    public bool boundedCamera = true;
    public Vector3 minValues = new Vector3(-100, -100, -100);
    public Vector3 maxValues = new Vector3(100, 100, 100);
    


    private void FixedUpdate() {
        CameraFollow();
    }

    private void CameraFollow() {
        Vector3 baseTargetPos = target.position + offset;
        Vector3 targetPos = boundedCamera ?
            new Vector3(Mathf.Clamp(baseTargetPos.x, minValues.x, maxValues.x),
                        Mathf.Clamp(baseTargetPos.y, minValues.y, maxValues.y),
                        Mathf.Clamp(baseTargetPos.z, minValues.z, maxValues.z))
            : baseTargetPos;
        Vector3 smoothedTransition = Vector3.Lerp(
            transform.position, targetPos, smoothScrollFactor * Time.fixedDeltaTime);
        transform.position = smoothedTransition;
    }
}
