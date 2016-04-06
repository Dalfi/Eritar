using UnityEngine;

namespace Eritar.Framework.Interfaces
{
  public interface ICanMove
  {
    Vector3 Destination { get; set; }

    float MovementSpeed { get; set; }

    float RotatingSpeed { get; set; }
  }
}
