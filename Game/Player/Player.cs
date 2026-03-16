using Godot;
using System;

public partial class Player : CharacterBody3D
{
    // =========================
    // Movement
    // =========================
    [ExportGroup("Movement")]
    [Export] public float MaxSpeed = 4.0f;
    [Export] public float Acceleration = 20.0f;
    [Export] public float Braking = 20.0f;
    [Export] public float AirAcceleration = 4.0f;
    [Export] public float JumpForce = 5.0f;
    [Export] public float GravityModifier = 1.5f;
    [Export] public float MaxRunSpeed = 6.0f;

    private bool _isRunning = false;

    // =========================
    // Camera
    // =========================
    [ExportGroup("Camera")]
    [Export] public float LookSensitivity = 0.005f;

    private Vector2 _cameraLookInput = Vector2.Zero;

    private Camera3D _camera;
    private float _gravity;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");

        _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity") * GravityModifier;

        // Lock mouse
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        float d = (float)delta;

        // Apply gravity
        if (!IsOnFloor())
        {
            Velocity = new Vector3(
                Velocity.X,
                Velocity.Y - _gravity * d,
                Velocity.Z
            );
        }

        // Jump
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            Velocity = new Vector3(Velocity.X, JumpForce, Velocity.Z);
        }

        // Movement input
        Vector2 moveInput = Input.GetVector("move_left", "move_right", "move_forward", "move_back");

        Vector3 moveDir =
            (Transform.Basis *
            new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();

        _isRunning = Input.IsActionPressed("sprint");

        float targetSpeed = MaxSpeed;

        if (_isRunning)
        {
            targetSpeed = MaxRunSpeed;

            float runDot = -moveDir.Dot(Transform.Basis.Z);
            runDot = Mathf.Clamp(runDot, 0.0f, 1.0f);
            moveDir *= runDot;
        }

        float currentSmoothing = Acceleration;

        if (!IsOnFloor())
        {
            currentSmoothing = AirAcceleration;
        }
        else if (moveDir == Vector3.Zero)
        {
            currentSmoothing = Braking;
        }

        Vector3 targetVel = moveDir * targetSpeed;

        Velocity = new Vector3(
            Mathf.Lerp(Velocity.X, targetVel.X, currentSmoothing * d),
            Velocity.Y,
            Mathf.Lerp(Velocity.Z, targetVel.Z, currentSmoothing * d)
        );

        MoveAndSlide();

        // =========================
        // Camera Look
        // =========================
        RotateY(-_cameraLookInput.X * LookSensitivity);

        _camera.RotateX(-_cameraLookInput.Y * LookSensitivity);

        Vector3 camRot = _camera.Rotation;
        camRot.X = Mathf.Clamp(camRot.X, -1.5f, 1.5f);
        _camera.Rotation = camRot;

        _cameraLookInput = Vector2.Zero;

        // Toggle mouse
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Visible)
                Input.MouseMode = Input.MouseModeEnum.Captured;
            else
                Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            _cameraLookInput = motion.Relative;
        }
    }
}
