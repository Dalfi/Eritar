using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public abstract class WorldObject
  {
    protected Player Owner;

    private string _objectName;
    private string _objectDescription;
    private string _iconFilePath;
    private string _objectGUID;

    private Vector3 position;

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

    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }

    public abstract void Update(float deltaTime);

    public void PrepareSaveObject()
    {
      if (this.ObjectGUID == null)
        this.ObjectGUID = System.Guid.NewGuid().ToString();
    }
  }
}
