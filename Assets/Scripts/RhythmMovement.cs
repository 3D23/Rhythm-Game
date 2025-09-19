using System;
using UnityEngine;
using VContainer;

public class RhythmMovement : MonoBehaviour
{
    public Action OnRhythmAccelerationChange;
    [Inject] private readonly ScenePresenter<RaceSceneData> presenter;
    private IChangeDataStrategy<RaceSceneData> changeDataStrategy;
    private bool isSprinted = false;
    private bool isSlowdown = false;
    private readonly float friction = 0.98f;
    private PlayerInputHandler inputHandler;
    private (byte Negative, byte Positive) moveButtonsClickedStatus = (0, 0);
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
            changeDataStrategy = new PlayerChangeDataStartegy(presenter);
            inputHandler.OnPositiveMoveAction += PositiveMoveActionHandler;
            inputHandler.OnNegativeMoveAction += NegativeMoveActionHandler;
            inputHandler.OnPositiveAccelerationAction += PositiveAccelerationActionHandler;
            inputHandler.OnNegativeAccelerationAction += NegativeAccelerationActionHandler;
        }
        else
        {
            changeDataStrategy = new EnemyChangeDataStrategy();
        }
        CalculateBounds();
        if (rhythmController != null)
            rhythmController.OnRhythmTick += OnRhythmTickHandler;
    }

    private void OnRhythmTickHandler(RhythmGradation gradation)
    {
        if (MathF.Abs(changeDataStrategy.Data.Acceleration.Value.x) < 1)
        {
            if (gradation == RhythmGradation.GOOD)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value += new Vector2(1, 0);
                });
            }
        }
        else
        {
            if (gradation == RhythmGradation.GOOD)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value *= new Vector2(1.5f, 1);
                });
            }
            else if (gradation == RhythmGradation.OK)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value /= new Vector2(1.5f, 1);
                });
            }
            else if (gradation == RhythmGradation.BAD)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value /= new Vector2(2, 1);
                });
            }
            else if (gradation == RhythmGradation.VERY_BAD)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value /= new Vector2(4, 1);
                });
            }
        }
        OnRhythmAccelerationChange?.Invoke();
    }

    public void SetAcceleration(Vector2 coeffVector)
    {
        if (coeffVector.x != 1)
        {
            if (changeDataStrategy.Data.Acceleration.Value.x == 0)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value += new Vector2(0.2f, 0);
                });
            }
        }
        if (coeffVector.y != 1)
        {
            if (changeDataStrategy.Data.Acceleration.Value.y == 0)
            {
                changeDataStrategy.SetData(data =>
                {
                    data.Acceleration.Value = new Vector2(data.Acceleration.Value.x, 0.2f);
                });
            }
        }
        changeDataStrategy.SetData(data =>
        {
            data.Acceleration.Value *= coeffVector;
        });
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

        if (changeDataStrategy.Data.Energy.Value < 0 + Mathf.Epsilon)
            isSprinted = false;

        if (!isSprinted)
        {
            changeDataStrategy.SetData(data =>
            {
                data.Energy.Value += Time.deltaTime * 4;
            });
        }

        if (isSprinted)
        {
            SetAcceleration(new Vector2(1.05f, 1));
            changeDataStrategy.SetData(data =>
            {
                data.Energy.Value -= Time.deltaTime * 40;
            });
        }

        float ac_y;
        if (moveButtonsClickedStatus.Positive - moveButtonsClickedStatus.Negative == 0)
        {
            ac_y = Mathf.MoveTowards(changeDataStrategy.Data.Acceleration.Value.y, 0f, Time.deltaTime);
        }
        else
        {
            ac_y = changeDataStrategy.Data.Acceleration.Value.y + (moveButtonsClickedStatus.Positive - moveButtonsClickedStatus.Negative) * 2f * Time.deltaTime;
        }
        changeDataStrategy.SetData(data =>
        {
            data.Acceleration.Value = new Vector2(data.Acceleration.Value.x, ac_y);
        });
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

    public void SetMaxSpeed(float value)
    {
        changeDataStrategy.SetData(data =>
        {
            data.MaxSpeed.Value = value;
        });
    }

    private void CalculateBounds()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    public void Stop()
    {
        changeDataStrategy.SetData(data =>
        {
            data.Acceleration.Value = Vector2.zero;
            data.Speed.Value = Vector2.zero;
        });
    }

    private void Move()
    {
        changeDataStrategy.SetData(data =>
        {
            data.Speed.Value += Time.deltaTime * data.Acceleration.Value;
            data.Speed.Value = new Vector2(
                Mathf.Clamp(data.Speed.Value.x, -data.MaxSpeed.Value, data.MaxSpeed.Value),
                Mathf.Clamp(data.Speed.Value.y, -data.MaxSpeed.Value, data.MaxSpeed.Value)
            );
            data.Speed.Value *= friction;
        });
        transform.Translate(Time.deltaTime * changeDataStrategy.Data.Speed.Value);
    }

    private void NegativeAccelerationActionHandler(bool isStarted)
    {
        if (isStarted)
            isSlowdown = true;
        else
            isSlowdown = false;
    }

    private void PositiveAccelerationActionHandler(bool isStarted)
    {
        if (isStarted)
        {
            if (changeDataStrategy.Data.Energy.Value >= changeDataStrategy.Data.EnergyThreshold.Value)
                isSprinted = true;
        }
        else
        {
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