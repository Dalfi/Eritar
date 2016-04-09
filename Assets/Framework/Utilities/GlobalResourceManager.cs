namespace Eritar.Framework.Utilities
{
  public class GlobalResourceManager
  {
    public static float BorderScrollOffset { get { return 15; } }
    public static float ScrollSpeed { get { return 35; } }
    public static float RotateSpeed { get { return 100; } }
    public static float RotateAmount { get { return 15; } }
    public static bool BorderScrollEnabled { get { return false; } }

    public const float MinCameraHeight = 2;
    public const float MaxCameraHeight = 40;
  }
}
