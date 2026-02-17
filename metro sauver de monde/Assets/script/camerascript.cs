using UnityEngine;

public class camerascript : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 0.25f, -0.5f);
    public bool offsetIsRelativeToTarget = true;
    [Tooltip("Multiplier for distance from target. Useful for adjusting camera distance based on model scale.")]
    [Range(0.1f, 3f)]
    public float distanceMultiplier = 1f;

    [Header("Camera Angle")]
    [Tooltip("Pitch angle in degrees. 0 = level, positive = look down, negative = look up")]
    [Range(-45f, 45f)]
    public float cameraTilt = 10f;
    [Tooltip("Height offset for look-at target")]
    public float lookAtHeightOffset = 1.2f;

    [Header("Smoothing")]
    [Tooltip("Lower = snappier. Typical: 0.1 - 0.4")]
    public float positionSmoothTime = 0.18f;
    [Tooltip("Higher = faster rotation responsiveness")]
    public float rotationSmoothSpeed = 10f;
    [Header("Physics Fallback")]
    [Tooltip("If the target uses a Rigidbody with no interpolation, use FixedUpdate sampling to reduce jitter.")]
    public bool useFixedUpdateFallback = true;

    Vector3 positionVelocity = Vector3.zero;

    Rigidbody targetRigidbody;
    bool warnedInterpolation = false;

    // Values sampled during FixedUpdate when using the fallback
    Vector3 fixedDesiredPosition;
    Quaternion fixedDesiredRotation;

    void FixedUpdate()
    {
        if (!useFixedUpdateFallback) return;
        if (target == null) return;
        if (targetRigidbody == null) targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody == null) return;
        if (targetRigidbody.interpolation != RigidbodyInterpolation.None) return;

        Vector3 scaledOffset = offset * distanceMultiplier;
        fixedDesiredPosition = offsetIsRelativeToTarget ? target.position + target.TransformDirection(scaledOffset) : target.position + scaledOffset;
        Vector3 lookAtTargetF = target.position + Vector3.up * lookAtHeightOffset;
        Quaternion baseLookRotation = Quaternion.LookRotation(lookAtTargetF - fixedDesiredPosition, Vector3.up);
        fixedDesiredRotation = baseLookRotation * Quaternion.Euler(cameraTilt, 0f, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (targetRigidbody == null) targetRigidbody = target.GetComponent<Rigidbody>();

        if (targetRigidbody != null && targetRigidbody.interpolation == RigidbodyInterpolation.None && !warnedInterpolation)
        {
            Debug.LogWarning("Target Rigidbody interpolation is disabled. Enable interpolation on the car's Rigidbody to reduce camera jitter.");
            warnedInterpolation = true;
        }

        Vector3 desiredPosition;
        Quaternion desiredRotation;

        if (useFixedUpdateFallback && targetRigidbody != null && targetRigidbody.interpolation == RigidbodyInterpolation.None)
        {
            desiredPosition = fixedDesiredPosition;
            desiredRotation = fixedDesiredRotation;
        }
        else
        {
            Vector3 scaledOffset = offset * distanceMultiplier;
            desiredPosition = offsetIsRelativeToTarget ? target.position + target.TransformDirection(scaledOffset) : target.position + scaledOffset;
            Vector3 lookAtTarget = target.position + Vector3.up * lookAtHeightOffset;
            Quaternion baseLookRotation = Quaternion.LookRotation(lookAtTarget - desiredPosition, Vector3.up);
            desiredRotation = baseLookRotation * Quaternion.Euler(cameraTilt, 0f, 0f);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, positionSmoothTime);

        float t = 1f - Mathf.Exp(-rotationSmoothSpeed * Time.deltaTime);
        Vector3 desiredEuler = desiredRotation.eulerAngles;
        Vector3 currentEuler = transform.rotation.eulerAngles;
        Vector3 smoothedEuler = new Vector3(
            Mathf.LerpAngle(currentEuler.x, desiredEuler.x, t),
            Mathf.LerpAngle(currentEuler.y, desiredEuler.y, t),
            Mathf.LerpAngle(currentEuler.z, desiredEuler.z, t)
        );
        transform.rotation = Quaternion.Euler(smoothedEuler);
    }
}
