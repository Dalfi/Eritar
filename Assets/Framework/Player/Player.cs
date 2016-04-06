using Eritar.Framework.Entities.General;
using System.Collections.Generic;

namespace Eritar.Framework
{
  public abstract class Player
  {
    private string _username;

    public List<Unit> Units { get; protected set; }
    public List<Building> Buildings { get; protected set; }
    protected List<Research> CompletedResearch { get; set; }

    public string Username
    {
      get { return _username; }
      set { _username = value; }
    }

    public virtual void Start()
    {
      Units = new List<Unit>();
      Buildings = new List<Building>();
      CompletedResearch = new List<Research>();
    }

    public bool CheckIfResearchIsResearched(string GUID)
    {
      if (CompletedResearch.Exists((t) => t.ObjectGUID == GUID))
        return true;
      else
        return false;
    }

    public bool CheckIfResearchIsResearched(List<string> lst)
    {
      bool result = true;

      foreach (string GUID in lst)
      {
        if (result == true)
          result = CheckIfResearchIsResearched(GUID);
        else
          break;
      }

      return result;
    }
    
  }
}