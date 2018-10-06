using System.Xml;
using UnityEngine;


public class NodeFactory : AttributeFactory
{
    /// <summary>
    /// Node id
    /// </summary>
    public ulong id { get; private set; }

    /// <summary>
    /// True if node is visible
    /// </summary>
    public bool visible { get; private set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public float lat { get; private set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public float lon { get; private set; }

    /// <summary>
    /// x coordinate
    /// </summary>
    public float x { get; private set; }

    /// <summary>
    /// y coordinate
    /// </summary>
    public float y { get; private set; }

    /// <summary>
    /// Implicit conversion between OsmNode and Vector3.
    /// </summary>
    /// <param name="nodeFactory">NodeFactory instance</param>
    public static implicit operator Vector3(NodeFactory nodeFactory)
    {
        return new Vector3(nodeFactory.x, 0, nodeFactory.y);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public NodeFactory(XmlNode node)
    {
        id = GetAttributes<ulong>("id", node.Attributes);
        // visible = GetAttributes<bool>("visible", node.Attributes);
        lat = GetAttributes<float>("lat", node.Attributes);
        lon = GetAttributes<float>("lon", node.Attributes);

        // Calculate x and y coordinates
        x = (float)MercatorProjection.lonToX(lon);
        y = (float)MercatorProjection.latToY(lat);
    }
}