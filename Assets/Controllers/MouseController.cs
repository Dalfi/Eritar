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
    private bool IsValidBuildingLocation = false;
    public Material allowedMaterial;
    public Material notAllowedMaterial;

    // Update is called once per frame
    void Update()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        Cursor.visible = true;
        return;
      }

      if (CurrentMode == MouseMode.BuildingPlacement)
      {
        if (Input.GetKey(KeyCode.Escape))
        {
          CurrentMode = MouseMode.Normal;
          Destroy(buildingPlacementTemplate);
          Cursor.visible = true;
        }
        else
        {
          Cursor.visible = false;
          buildingPlacementTemplate.transform.position = FindHitPoint(Input.mousePosition);

          if (Input.GetKey(KeyCode.LeftShift))
            buildingPlacementTemplate.transform.Rotate(0, GlobalResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel"), 0);
          else if (Input.GetKeyDown(KeyCode.R))
            buildingPlacementTemplate.transform.Rotate(0, 45f, 0);

          if (CheckBuildLocation())
          {
            foreach(MeshRenderer renderer in buildingPlacementTemplate.GetComponentsInChildren<MeshRenderer>())
            {
              renderer.material = allowedMaterial;
            }
          }
          else
          {
            foreach (MeshRenderer renderer in buildingPlacementTemplate.GetComponentsInChildren<MeshRenderer>())
            {
              renderer.material = notAllowedMaterial;
            }
          }
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
        if (IsValidBuildingLocation)
        {
          Player p = this.gameObject.GetComponent<PlayerController>().Player;
          Building b = GameDataController.Instance.GetBuildingByName(buildingPlacementTemplate.tag);

          if (p != null && b != null)
            WorldController.Instance.world.CreateBuilding(b, p, buildingPlacementTemplate.transform.position, buildingPlacementTemplate.transform.rotation);

          CurrentMode = MouseMode.Normal;
          Destroy(buildingPlacementTemplate);
          Cursor.visible = true;
        }
        else
        {
          //Play Error Sound or so ?
        }
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
      Cursor.visible = false;

      Vector3 m = Input.mousePosition;
      m = new Vector3(m.x, m.y, Camera.main.transform.position.y);
      Vector3 p = Camera.main.ScreenToWorldPoint(m);

      GameObject prefab = BuildingController.Instance.GetPrefab(tag);

      buildingPlacementTemplate = (GameObject)Instantiate(prefab, new Vector3(p.x, 0.1f, p.z), prefab.transform.rotation);
      buildingPlacementTemplate.layer = 2;//Ignore Raycast

      foreach (GameObject item in buildingPlacementTemplate.GetChildren())
        item.layer = 2;
    }

    private Vector3 FindHitPoint(Vector3 origin)
    {
      //declare a variable of RaycastHit struct
      RaycastHit hit;
      Ray ray;
      Vector3 endPoint = Vector3.zero;

      //Create a Ray on mouse position
      ray = Camera.main.ScreenPointToRay(origin);

      int layerMask = 1 << 2;
      layerMask = ~layerMask;

      if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
      {
          //save the click position
          endPoint = hit.point;

          //as we do not want to change the y axis value based on the input position, reset it to original y axis value
          if (buildingPlacementTemplate != null)
            endPoint.y = buildingPlacementTemplate.transform.position.y;
          else
            endPoint.y = 0.1f;
      }

      return endPoint;
    }

    private bool CheckBuildLocation()
    {
      IsValidBuildingLocation = true;

      BoxCollider col = buildingPlacementTemplate.GetComponent<BoxCollider>();

      Bounds placeBounds = col.bounds;

      foreach (Vector3 corner in placeBounds.GetCorners())
      {
        GameObject hitObject = FindHitObject(corner);

        if (hitObject != null && hitObject.tag != "Ground")
        {
          BoxCollider hitCol = hitObject.GetComponent<BoxCollider>();

          if ( placeBounds.Intersects(hitCol.bounds))
            IsValidBuildingLocation = false;
        }
      }

      return IsValidBuildingLocation;
    }

    public static GameObject FindHitObject(Vector3 origin)
    {
      Ray ray = Camera.main.ScreenPointToRay(origin);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
        return hit.collider.gameObject;

      return null;
    }
  }
}