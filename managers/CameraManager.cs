using Godot;
using System;

public class CameraManager : Node2D
{
    private Camera _camera;

    public static float ZoomFactor = 1;

    public override void _Ready()
    {
    }

    public void Init(Camera camera)
    {
        _camera = camera;
        _camera.Zoom = new Vector2(1 / ZoomFactor, 1 / ZoomFactor);
    }

    public void AddTrauma(float amount, float maxTrauma = -1f)
    {
        _camera.AddTrauma(amount, maxTrauma);
    }

}