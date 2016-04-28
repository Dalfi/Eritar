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
    public static MouseController Instance { get; protected set; }

    private GameObject buildingPlacementTemplate { get; set; }
    private bool IsValidBuildingLocation = false;

    public MouseMode CurrentMode { get; set; }
    public Material allowedMaterial;
    public Material notAllowedMaterial;
    private Player _player;

    private Player player
    {
      get
      {
        if(_player == null)
        _player = this.gameObject.GetComponent<PlayerController>().Player;

        return _player;
      }
    }

    // Use this for initialization
    void OnEnable()
    {
      if (Instance != null)
      {
        Debug.LogError("There should never be two mouse controllers.");
      }

      Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        Cursor.visible = true;
        return;
      }

      if (CurrentMode == MouseMode.BuildingPlacement)
        HandleBuildmode();

      if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        LeftMouseClick();
      else if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
        RightMouseClick();
    }

    private void LeftMouseClick()
    {
      if (CurrentMode == MouseMode.BuildingPlacement)
      {
        #region BuilingPlacement
        if (IsValidBuildingLocation)
        {
          Building b = GameDataController.Instance.GetBuildingByName(buildingPlacementTemplate.tag);

          if (player != null && b != null)
            WorldController.Instance.world.CreateBuilding(b, player, buildingPlacementTemplate.transform.position, buildingPlacementTemplate.transform.rotation);

          if (!Input.GetKey(KeyCode.LeftShift))
          {
            CurrentMode = MouseMode.Normal;
            Destroy(buildingPlacementTemplate);
            Cursor.visible = true;
          }
        }
        else
        {
          //Play Error Sound or so ?
        }
        #endregion
      }
      else
      {
        GameObject hitObject = FindHitObject(Input.mousePosition);

        if (hitObject != null && hitObject.tag != "Ground")
        {
          if (WorldController.Instance.WorldObjectSelected(hitObject))
            return;
          else
          {
            WorldController.Instance.ClearWorldObjectSelection();
          }
        }
        else
        {
          // clear selection
          WorldController.Instance.ClearWorldObjectSelection();
        }
      }
    }

    private void RightMouseClick()
    {

    }

    private void HandleBuildmode()
    {
      if (Input.GetKey(KeyCode.Escape))
      {
        CurrentMode = MouseMode.Normal;
        Destroy(buildingPlacementTemplate);
        Cursor.visible = true;
      }
      else
      {
        bool HasMoved = false;
        Cursor.visible = false;
        Vector3 newpos = FindHitPoint(Input.mousePosition);

        if (newpos != buildingPlacementTemplate.transform.position)
        {
          HasMoved = true;
          buildingPlacementTemplate.transform.position = newpos;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
          HasMoved = true;
          buildingPlacementTemplate.transform.Rotate(0, GlobalResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel"), 0);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
          HasMoved = true;
          buildingPlacementTemplate.transform.Rotate(0, 45f, 0);
        }

        if (!HasMoved)
          return;

        if (CheckBuildLocation())
        {
          foreach (MeshRenderer renderer in buildingPlacementTemplate.GetComponentsInChildren<MeshRenderer>())
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

    public void BeginBuildingPlacement(string tag)
    {
      if (buildingPlacementTemplate != null)
        Destroy(buildingPlacementTemplate);

      CurrentMode = MouseMode.BuildingPlacement;
      Cursor.visible = false;

      Vector3 m = Input.mousePosition;
      m = new Vector3(m.x, m.y, CameraController.Instance.myCamera.transform.position.y);
      Vector3 p = CameraController.Instance.myCamera.ScreenToWorldPoint(m);

      GameObject prefab = BuildingController.Instance.GetPrefab(tag);

      buildingPlacementTemplate = (GameObject)Instantiate(prefab, new Vector3(p.x, -0.9f, p.z), prefab.transform.rotation);
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
      ray = CameraController.Instance.myCamera.ScreenPointToRay(origin);

      int layerMask = 1 << 8;
      //layerMask = 1 << 2;
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
      Bounds hitBounds;

      foreach (Vector3 corner in placeBounds.GetCorners(CameraController.Instance.myCamera))
      {
        GameObject hitObject = FindHitObject(corner);

        if (hitObject != null && hitObject.tag != "Ground")
        {
          BoxCollider hitCol = hitObject.GetComponent<BoxCollider>();

          if (hitCol == null && hitObject.transform.parent.tag != "Ground")
          {
            hitCol = hitObject.GetComponentInParent<BoxCollider>();
          }
          if (hitCol != null)
          {
            hitBounds = hitCol.bounds;

            hitBounds.Expand(2f);
            placeBounds.Expand(2f);

            if (placeBounds.Intersects(hitBounds))
              IsValidBuildingLocation = false;
          }
        }
      }

      return IsValidBuildingLocation;
    }

    private GameObject FindHitObject(Vector3 origin)
    {
      Ray ray = CameraController.Instance.myCamera.ScreenPointToRay(origin);
      RaycastHit hit;

      int layerMask = 1 << 2;
      layerMask = ~layerMask;

      if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        return hit.collider.gameObject;

      return null;
    }
  }
}