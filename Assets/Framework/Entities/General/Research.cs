using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Research : WorldObject, INeedResearch
  {
    public float ResearchTime { get; protected set; }

    public List<string> NeededResearch { get; set; }

    public bool IsNeededResearchCompleted
    {
      get
      {
        return this.Owner.CheckIfResearchIsResearched(NeededResearch);
      }
    }

    public static bool SaveResearch(Research obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(Research));

      using (TextWriter writer = new StreamWriter(FilePath))
      {
        // Serializes the purchase order, and closes the TextWriter.
        serializer.Serialize(writer, obj);
        writer.Close();
      }

      return true;
    }

    public static Research LoadResearchFromXML(string FilePath)
    {
      Research obj;

      XmlSerializer serializer = new XmlSerializer(typeof(Research));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      using (FileStream fs = new FileStream(FilePath, FileMode.Open))
      {
        obj = (Research)serializer.Deserialize(fs);
      }

      return obj;
    }

    protected static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
    {
      Debug.LogError("Unknown Node:" + e.Name + "\t" + e.Text);
    }

    protected static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {
      System.Xml.XmlAttribute attr = e.Attr;
      Debug.LogError("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
    }

    public override void Update(float deltaTime)
    {
      if(ResearchTime > 0)
      {
        ResearchTime = ResearchTime - deltaTime;
      }
    }
  }
}