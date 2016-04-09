using Eritar.Framework;
using Eritar.Framework.Entities.General;
using Eritar.Framework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public enum MouseMode
{
  Normal,
  BuildingPlacement
}

namespace Eritar
{
  public class MouseController : MonoBehaviour
  {
    public MouseMode CurrentMode { get; set; }
    private GameObject buildingPlacementTemplate { get; set; }

    // Update is called once per frame
    void Update()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        return;
      }

      if (CurrentMode == MouseMode.BuildingPlacement)
      {
        if (Input.GetKey(KeyCode.Escape))
        {
          CurrentMode = MouseMode.Normal;
          Destroy(buildingPlacementTemplate);
        }
        else
        {
          buildingPlacementTemplate.transform.position = FindHitPoint();

          if (Input.GetKey(KeyCode.LeftShift))
            buildingPlacementTemplate.transform.Rotate(0, GlobalResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel"), 0);
          else if(Input.GetKeyDown(KeyCode.R))
            buildingPlacementTemplate.transform.Rotate(0, 45f, 0);
        }
      }

      if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        LeftMouseClick();
      else if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
        RightMouseClick();
    }

    private void LeftMouseClick()
    {
      if (CurrentMode == MouseMode.BuildingPlacement)
      {
        Player p = this.gameObject.GetComponent<PlayerController>().Player;
        Building b = GameDataController.Instance.GetBuildingByName(buildingPlacementTemplate.tag);

        if (p != null && b != null)
          WorldController.Instance.world.CreateBuilding(b, p, buildingPlacementTemplate.transform.position, buildingPlacementTemplate.transform.rotation);

        CurrentMode = MouseMode.Normal;
        Destroy(buildingPlacementTemplate);
      }
    }

    private void RightMouseClick()
    {

    }


    public void BeginBuildingPlacement(string tag)
    {
      if (buildingPlacementTemplate != null)
        Destroy(buildingPlacementTemplate);

      CurrentMode = MouseMode.BuildingPlacement;

      Vector3 m = Input.mousePosition;
      m = new Vector3(m.x, m.y, Camera.main.transform.position.y);
      Vector3 p = Camera.main.ScreenToWorldPoint(m);

      GameObject prefab = BuildingController.Instance.GetPrefab(tag);

      buildingPlacementTemplate = (GameObject)Instantiate(prefab, new Vector3(p.x, 0, p.z), prefab.transform.rotation);

      foreach (MeshCollider col in buildingPlacementTemplate.GetComponentsInChildren<MeshCollider>())
      {
        col.enabled = false;
      }
    }

    private Vector3 FindHitPoint()
    {
      //declare a variable of RaycastHit struct
      RaycastHit hit;
      Ray ray;
      Vector3 endPoint = Vector3.zero;

      //Create a Ray on mouse position
      ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit))
      {
        //save the click position
        endPoint = hit.point;

        //as we do not want to change the y axis value based on the input position, reset it to original y axis value
        if (buildingPlacementTemplate != null)
          endPoint.y = buildingPlacementTemplate.transform.position.y;
        else
          endPoint.y = 0;
      }

      return endPoint;
    }
  }
}