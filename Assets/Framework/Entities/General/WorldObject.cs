using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public abstract class WorldObject
  {
    private Player _owner;

    private string _objectName;
    private string _objectDescription;
    private string _iconFilePath;
    private string _objectGUID;
    private bool _isSelected;

    private Vector3 position;
    private Quaternion rotation;

    public string ObjectName
    {
      get { return _objectName; }
      set { _objectName = value; }
    }
    public string ObjectDescription
    {
      get { return _objectDescription; }
      set { _objectDescription = value; }
    }

    public string IconFilePath
    {
      get { return _iconFilePath; }
      set { _iconFilePath = value; }
    }

    public string ObjectGUID
    {
      get { return _objectGUID; }
      set { _objectGUID = value; }
    }

    public bool IsSelected
    {
      get { return _isSelected; }
      set { _isSelected = value; }
    }

    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }

    public Quaternion Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }

    public Player Owner
    {
      get { return _owner; }
      set { _owner = value; }
    }

    public WorldObject()
    {
      IsSelected = false;
    }

    public abstract void Update(float deltaTime);

    public void PrepareSaveObject()
    {
      if (this.ObjectGUID == null)
        this.ObjectGUID = System.Guid.NewGuid().ToString();
    }
  }
}
