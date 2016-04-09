using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Eritar.Framework.Utilities
{
  public static class UnityExtensions
  {
    public static IEnumerable<GameObject> GetChildren(this GameObject go)
    {
      List<Transform> lst = new List<Transform>();

      UnityExtensions.GetChildrenImpl(go.transform, ref lst);

      return (from child in lst select child.gameObject);
    }
    
    private static void GetChildrenImpl(Transform parent, ref List<Transform> lst)
    {
      if (lst == null)
        lst = new List<Transform>();

      if (parent.childCount <= 0)
        return;

      foreach (Transform child in parent)
      {
        lst.Add(child);

        UnityExtensions.GetChildrenImpl(child, ref lst);
      }
    }

    public static List<Vector3> GetCorners(this Bounds bounds)
    {    
      //shorthand for the coordinates of the center of the selection bounds
      float cx = bounds.center.x;
      float cy = bounds.center.y;
      float cz = bounds.center.z;
      //shorthand for the coordinates of the extents of the selection box
      float ex = bounds.extents.x;
      float ey = bounds.extents.y;
      float ez = bounds.extents.z;

      //Determine the screen coordinates for the corners of the selection bounds
      List<Vector3> corners = new List<Vector3>();
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz + ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz - ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz + ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz + ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz - ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz + ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz - ez)));
      corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz - ez)));

      return corners;
    }
  }
}
