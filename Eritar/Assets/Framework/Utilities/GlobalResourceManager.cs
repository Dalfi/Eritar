using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eritar.Framework.Utilities
{
  public static class GlobalResourceManager
  {
    public static bool BorderScrollEnabled { get; private set; }

    public static float ScrollSpeed { get; private set; }
    public static float BorderScrollOffset { get; private set; }

    public const float MinCameraHeight = 7.5f;
    public const float MaxCameraHeight = 40;

    static GlobalResourceManager()
    {
      BorderScrollEnabled = false;
      ScrollSpeed = 25f;
      BorderScrollOffset = 15f;
    }

  }
}
