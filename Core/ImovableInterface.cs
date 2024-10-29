using UnityEngine;

public interface IMovable
{
    Vector3 Position { get; set; }
    float MoveSpeed { get; set; }
}
