using System;
using UnityEngine;

public class RhythmMovement : MonoBehaviour
{
    public readonly float MaxEnergy = 100;
    public readonly float EnergyThreshold = 20;
    public float Energy
    {
        get => energy;
        private set => energy = Math.Clamp(value, 0, MaxEnergy);
    }

    private bool isSprinted = false;
    private bool isSlowdown = false;

    public Vector2 Speed
    {
        get => speed;
        private set => speed = new Vector2(Mathf.Clamp(value.x, 0, maxSpeed), value.y);
    }

    public Vector2 Acceleration
    {
        get => acceleration;
        private set => acceleration = new Vector2(Mathf.Clamp(value.x, -maxAcel, maxAcel), Mathf.Clamp(value.y, -maxAcel, maxAcel));
    }

    private Vector2 speed;
    private Vector2 acceleration;
    private float energy = 100;

    private float maxSpeed = 1.1f;
    private readonly float maxSpeedBeforeSprint = 1.1f;
    private readonly float maxSpeedAfterSprint = 1.1f * 2;
    private readonly float maxAcel = 20f;
    private readonly float friction = 0.98f;

    private PlayerInputHandler inputHandler;

    private (byte Negative, byte Positive) moveButtonsClickedStatus = (0, 0);
    private (byte Negative, byte Positive) accelerationButtonsClickedStatus = (0, 0);

    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private RhythmController rhythmController;

    #region Unity Lifecycle
    private void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rhythmController = GetComponent<RhythmController>();
        mainCamera = Camera.main;
        if (inputHandler != null) 
        {
            inputHandler.OnPositiveMoveAction += PositiveMoveActionHandler;
            inputHandler.OnNegativeMoveAction += NegativeMoveActionHandler;
            inputHandler.OnPositiveAccelerationAction += PositiveAccelerationActionHandler;
            inputHandler.OnNegativeAccelerationAction += NegativeAccelerationActionHandler;
        }
        CalculateBounds();
        if (rhythmController != null)
            rhythmController.OnRhythmTick += OnRhythmTickHandler;
    }

    private void OnRhythmTickHandler(RhythmGradation gradation)
    {
        if (MathF.Abs(acceleration.x) < 1)
        {
            if (gradation == RhythmGradation.GOOD)
                Acceleration += new Vector2(1, 0);
        }
        else
        {
            if (gradation == RhythmGradation.GOOD)
                Acceleration *= new Vector2(1.5f, 1);
            else if (gradation == RhythmGradation.OK)
                Acceleration /= new Vector2(1.5f, 1);
            else if (gradation == RhythmGradation.BAD)
                Acceleration /= new Vector2(2, 1);
            else if (gradation == RhythmGradation.VERY_BAD)
                Acceleration /= new Vector2(4, 1);
        }
    }

    public void SetAcceleration(Vector2 coeffVector)
    {
        if (coeffVector.x != 1)
        {
            if (acceleration.x == 0)
                Acceleration = new Vector2(0.2f, acceleration.y);
        }
        if (coeffVector.y != 1)
        {
            if (acceleration.y == 0)
                Acceleration = new Vector2(acceleration.x, 0.2f);
        }
        Acceleration *= coeffVector;
    }

    private void OnDestroy()
    {
        if (inputHandler != null) 
        {
            inputHandler.OnPositiveMoveAction -= PositiveMoveActionHandler;
            inputHandler.OnNegativeMoveAction -= NegativeMoveActionHandler;
            inputHandler.OnPositiveAccelerationAction -= PositiveAccelerationActionHandler;
            inputHandler.OnNegativeAccelerationAction -= NegativeAccelerationActionHandler;
        }
        if (rhythmController != null)
            rhythmController.OnRhythmTick -= OnRhythmTickHandler;
    }

    private void Update()
    {
        if (isSlowdown)
            SetAcceleration(new Vector2(0.3f, 1));

        if (energy < 0 + Mathf.Epsilon)
            isSprinted = false;

        if (!isSprinted)
        {
            Energy += Time.deltaTime * 4;
            SetMaxSpeed(maxSpeedBeforeSprint);
        }

        if (isSprinted)
        {
            SetMaxSpeed(maxSpeedAfterSprint);
            SetAcceleration(new Vector2(1.05f, 1));
            Energy -= Time.deltaTime * 40;
        }

        float ac_y;
        if (moveButtonsClickedStatus.Positive - moveButtonsClickedStatus.Negative == 0)
        {
            ac_y = Mathf.MoveTowards(acceleration.y, 0f, Time.deltaTime);
        }
        else
        {
            ac_y = acceleration.y + (moveButtonsClickedStatus.Positive - moveButtonsClickedStatus.Negative) * 2f * Time.deltaTime;
        }
        Acceleration = new Vector2(acceleration.x, ac_y);
        Move();
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
        if (transform.position.x > screenBounds.x - objectWidth || transform.position.x < -screenBounds.x + objectWidth ||
            transform.position.y > screenBounds.y - objectHeight || transform.position.y < -screenBounds.y + objectHeight)
            Stop();
        transform.position = viewPos;
    }
    #endregion

    public void SetMaxSpeed(float value) => 
        maxSpeed = value;

    private void CalculateBounds()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    public void Stop()
    {
        Acceleration = Vector2.zero;
        Speed = Vector2.zero;
    }

    private void Move()
    {
        Speed += Time.deltaTime * acceleration;
        Speed = new Vector2(Mathf.Clamp(speed.x, -maxSpeed, maxSpeed), Mathf.Clamp(speed.y, -maxSpeed, maxSpeed));
        Speed *= friction;
        transform.Translate(Time.deltaTime * speed);
    }

    private void NegativeAccelerationActionHandler(bool isStarted)
    {
        if (isStarted)
        {
            accelerationButtonsClickedStatus.Negative = 1;
            isSlowdown = true;
        }
        else
        {
            accelerationButtonsClickedStatus.Negative = 0;
            isSlowdown = false;
        }
    }

    private void PositiveAccelerationActionHandler(bool isStarted)
    {
        if (isStarted)
        {
            accelerationButtonsClickedStatus.Positive = 1;
            if (energy >= EnergyThreshold)
                isSprinted = true;
        }
        else
        {
            accelerationButtonsClickedStatus.Negative = 0;
            isSprinted = false;
        }
    }

    private void NegativeMoveActionHandler(bool isStarted)
    {
        if (isStarted) moveButtonsClickedStatus.Negative = 1;
        else moveButtonsClickedStatus.Negative = 0;
    }

    private void PositiveMoveActionHandler(bool isStarted)
    {
        if (isStarted) moveButtonsClickedStatus.Positive = 1;
        else moveButtonsClickedStatus.Positive = 0;
    }
}