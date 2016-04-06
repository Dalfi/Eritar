using UnityEngine;

namespace Eritar.Framework.Utilities
{
  public class GUIHelper : MonoBehaviour
  {
    public void ToggleEnableDisable()
    {
      GameObject go = this.gameObject;
      if (go.activeInHierarchy)
        go.SetActive(false);
      else
        go.SetActive(true);
    }
  }
}
