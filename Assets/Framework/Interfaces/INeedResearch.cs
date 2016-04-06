using System.Collections.Generic;

namespace Eritar.Framework
{
  public interface INeedResearch
  {
    List<string> NeededResearch { get; set; }

    bool IsNeededResearchCompleted { get; }
  }
}
