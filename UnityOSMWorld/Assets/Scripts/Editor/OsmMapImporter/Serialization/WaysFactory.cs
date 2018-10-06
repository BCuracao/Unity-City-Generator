using System.Collections.Generic;
using System.Xml;


public class WaysFactory : AttributeFactory
{
    /// <summary>
    /// Way id
    /// </summary>
    public ulong id { get; private set; }

    /// <summary>
    /// Name of way
    /// </summary>
    public string name { get; private set; }

    /// <summary>
    /// True if way is building
    /// </summary>
    public bool isBuilding { get; private set; }

    /// <summary>
    /// True if way is visible
    /// </summary>
    public bool isVisible { get; private set; }

    /// <summary>
    /// Building height
    /// </summary>
    public float height { get; private set; }

    /// <summary>
    /// List of way IDs.
    /// </summary>
    public List<ulong> ndref { get; private set; }

    /// <summary>
    /// True if way is highway
    /// </summary>
    public bool isHighway { get; private set; }

    /// <summary>
    /// True if way is park
    /// </summary>
    public bool isPark { get; private set; }

    /// <summary>
    /// True if way is waterway
    /// </summary>
    public bool isWaterway { get; private set; }

    /// <summary>
    /// True if way is stream
    /// </summary>
    public bool isStream { get; private set; }

    /// <summary>
    /// True if way is footway
    /// </summary>
    public bool isFootway { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public WaysFactory(XmlNode xmlNode)
    {
        id = GetAttributes<ulong>("id", xmlNode.Attributes);
        //isVisible = GetAttributes<bool>("visible", xmlNode.Attributes);
        ndref = new List<ulong>();

        // Default height if no height available
        height = 12.5f;

        // Default name if no name available
        name = "noData";

        // Get all nd ref sub-nodes
        XmlNodeList nd = xmlNode.SelectNodes("nd");
        foreach (XmlNode n in nd)
        {
            ndref.Add(GetAttributes<ulong>("ref", n.Attributes));
        }

        // Read and store the values for of the tag elements in the XML
        XmlNodeList tag = xmlNode.SelectNodes("tag");
        foreach (XmlNode n in tag)
        {
            string k = GetAttributes<string>("k", n.Attributes);

            if (k == "building")
            {
                isBuilding = true;
            }
            else if (k == "height")
            {
                height = 2.0f * GetWayAttributes<float>("v", n.Attributes);
            }
            //if (k == "building:levels")
            //{
            //    height = 8.0f * GetAttributes<float>("v", n.Attributes);
            //}
            else if (k == "name")
            {
                name = GetAttributes<string>("v", n.Attributes);
            }
            else if (k == "highway")
            {
                string value = GetAttributes<string>("v", n.Attributes);
                if (value != "bus_stop" && value != "foot" && value != "footway" && value != "path" && value != "pedestrian")
                {
                    isHighway = true;
                }
                if (value == "footway" || value == "pedestrian")
                {
                    isFootway = true;
                }
            }
            else if (k == "leisure")
            {
                string value = GetAttributes<string>("v", n.Attributes);
                if (value == "park")
                {
                    isPark = true;
                }
            }
            else if (k == "waterway")
            {
                string value = GetAttributes<string>("v", n.Attributes);
                if (value == "river")
                {
                    isWaterway = true;
                }
                else if(value == "stream")
                {
                    isStream = true;
                }
            }
        }
    }
}