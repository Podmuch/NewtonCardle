using UnityEngine;

public class NewtonCardle : MonoBehaviour
{
    public CardleBall[] Balls;

    public bool IsPlaying = false;
    public int maxCollisionIterations = 8;

    private bool collisionFound = false;
    private int doneIterations = 0;
    private float signOfTimeScale = 1.0f;
    private float initialInclination = 0.0f;

    private void FixedUpdate()
    {
        if(IsPlaying)
        {
            CalculateMovement();
            doneIterations = 0;
            do
            {
                doneIterations++;
                collisionFound = HandleCollisions();
            }
            while (collisionFound && doneIterations < maxCollisionIterations);
            ChangePositions();
        }
    }

    public void Init(float inclination, int ballsUp)
    {
        IsPlaying = false;
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].Acceleration = Balls[i].Speed = 0;
            Balls[i].Mass = 1;
            Balls[i].CurrentAngle = Balls[i].NextAngle = i < ballsUp ? Mathf.Lerp(0, -90, inclination) : 0;
        }
        initialInclination = Mathf.Abs(Balls[0].NextAngle);
    }

    public void Play()
    {
        signOfTimeScale = 1.0f;
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = !IsPlaying;
    }

    public void SetPlaySpeed(float timeValue)
    {
        signOfTimeScale = Mathf.Sign(timeValue);
        Time.timeScale = Mathf.Abs(timeValue);
        signOfTimeScale = Mathf.Sign(timeValue);
    }

    private void CalculateMovement()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].Acceleration = Physics.gravity.y * CardleBall.L * Mathf.Sin(Balls[i].CurrentAngle * Mathf.Deg2Rad);
            Balls[i].NextAngle = Balls[i].CurrentAngle + signOfTimeScale * Balls[i].Speed * Time.fixedDeltaTime + signOfTimeScale * Balls[i].Acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f;
            Balls[i].NextAngle = Mathf.Clamp(Balls[i].NextAngle > 180 ? Balls[i].NextAngle - 360 : Balls[i].NextAngle, -initialInclination, initialInclination);
            Balls[i].Speed += signOfTimeScale * Balls[i].Acceleration * Time.fixedDeltaTime;
        }
    }

    private bool HandleCollisions()
    {
        float firstBallAngle = 0, secondBallAngle = 0, distanceBefore= 0, distanceAfter = 0, tempSpeed = 0;
        Vector3 firstBallPosition = Vector3.zero, secondBallPosition = Vector3.zero;
        for (int i = 0; i < Balls.Length -1; i++)
        {
            firstBallAngle = Balls[i].CurrentAngle;
            Balls[i].CurrentAngle = Balls[i].NextAngle;
            firstBallPosition = Balls[i].BallTransform.position;
            //
            secondBallAngle = Balls[i+1].CurrentAngle;
            Balls[i+1].CurrentAngle = Balls[i+1].NextAngle;
            secondBallPosition = Balls[i + 1].BallTransform.position;
            distanceAfter = Vector3.Distance(Balls[i].BallTransform.position, Balls[i + 1].BallTransform.position);
            //
            Balls[i].CurrentAngle = firstBallAngle;
            Balls[i + 1].CurrentAngle = secondBallAngle;
            distanceBefore = Vector3.Distance(Balls[i].BallTransform.position, Balls[i + 1].BallTransform.position);
            if (distanceAfter < CardleBall.SIZE && distanceBefore != distanceAfter)
            {
                float coveredDistance = distanceBefore - distanceAfter;
                float overlappedDistance = CardleBall.SIZE - distanceAfter;
                float overlappedPart = 1.0f - overlappedDistance / coveredDistance;
                float distanceInAngleFirst = Mathf.DeltaAngle(Balls[i].CurrentAngle, Balls[i].NextAngle);
                float distanceInAngleSecond = Mathf.DeltaAngle(Balls[i + 1].CurrentAngle, Balls[i+1].NextAngle);
                float coveredAngleByFirst = overlappedPart * distanceInAngleFirst;
                float coveredAngleBySecond = overlappedPart * distanceInAngleSecond;
                float nextAngleFirst = Balls[i].CurrentAngle + coveredAngleByFirst + distanceInAngleSecond - coveredAngleBySecond;
                float nextAngleSecond = Balls[i+1].CurrentAngle + coveredAngleBySecond + distanceInAngleFirst - coveredAngleByFirst;
                Balls[i].NextAngle = nextAngleFirst;
                Balls[i+1].NextAngle = nextAngleSecond;
                //
                tempSpeed = Balls[i].Speed;
                Balls[i].Speed = Balls[i + 1].Speed;
                Balls[i + 1].Speed = tempSpeed;
                return true;
            }
        }
        return false;
    }

    private void ChangePositions()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].CurrentAngle = Balls[i].NextAngle;
        }
    }
}
