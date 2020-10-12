using Godot;

public class CameraController : KinematicBody
{
    [Export] public float CameraSpeed = 1.0f;
    [Export] public float CameraAcceleraton = 1.0f;
    [Export] public float CameraDeacceleration = 1.0f;
    [Export] public float MouseSensitivity = 0.05f;

    private Vector3 _velocity  = new Vector3();
    private Vector3 _direction;
    
    private Camera  _camera;
    private Spatial _rotationHelper;
    
    public override void _Ready()
    {
        _camera         = GetNode<Camera>("RotationHelper/Camera");
        _rotationHelper = GetNode<Spatial>("RotationHelper");
        
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _PhysicsProcess(float delta)
    {
        ProcessInput(delta);
        ProcessMovement(delta);
    }

    private void ProcessInput(float delta)
    {
        _direction = new Vector3();
        Transform cameraTransform = _camera.GlobalTransform;
        
        Vector3 inputMovementVector = new Vector3();

        if (Input.IsActionPressed("MoveForward"))
            inputMovementVector.z -= 1;
        if (Input.IsActionPressed("MoveBack"))
            inputMovementVector.z += 1;
        if (Input.IsActionPressed("MoveLeft"))
            inputMovementVector.x -= 1;
        if (Input.IsActionPressed("MoveRight"))
            inputMovementVector.x += 1;
        if (Input.IsActionPressed("MoveUp"))
            inputMovementVector.y += 1;
        if (Input.IsActionPressed("MoveDown"))
            inputMovementVector.y -= 1;

        inputMovementVector = inputMovementVector.Normalized();

        _direction += cameraTransform.basis.z * inputMovementVector.z;
        _direction += cameraTransform.basis.x * inputMovementVector.x;
        _direction += cameraTransform.basis.y * inputMovementVector.y;
    }

    private void ProcessMovement(float delta)
    {
        _direction = _direction.Normalized();
        Vector3 target = _direction * CameraSpeed;

        float acceleration = (_direction.Dot(_velocity) > 0) ? CameraAcceleraton : CameraDeacceleration;

        _velocity = MoveAndSlide(_velocity.LinearInterpolate(target, acceleration * delta));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            _rotationHelper.RotateX(Mathf.Deg2Rad(-mouseEvent.Relative.y * MouseSensitivity));
            RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * MouseSensitivity));

            Vector3 cameraRotation = _rotationHelper.RotationDegrees;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -70, 70);
            _rotationHelper.RotationDegrees = cameraRotation;
        }
    }
}
