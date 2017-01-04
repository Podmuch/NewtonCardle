using UnityEngine;
using System.Collections;

public class CardleBall : MonoBehaviour
{
    public const float L = 3;
    public const float SIZE = 1;

    public Transform HandleTransform;
    public Transform BallTransform;

    public float CurrentAngle
    {
        get
        {
            return HandleTransform.localRotation.eulerAngles.z;
        }
        set
        {
            HandleTransform.localRotation = Quaternion.Euler(0, 0, value);
        }
    }
    public float NextAngle { get; set; }
    public float Mass { get; set; }
    public float Acceleration { get; set; }
    public float Speed { get; set; }
}
