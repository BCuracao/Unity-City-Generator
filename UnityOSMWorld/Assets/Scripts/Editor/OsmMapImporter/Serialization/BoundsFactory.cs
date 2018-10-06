using System.Xml;
using UnityEngine;

/// <summary>
/// Bounds of the map
/// </summary>
public class BoundsFactory : AttributeFactory
{
    /// <summary>
    /// Minimum latitude
    /// </summary>
    public float minlat { get; private set; }

    /// <summary>
    /// Mnimum longitude
    /// </summary>
    public float minlon { get; private set; }

    /// <summary>
    /// Maximum latitude
    /// </summary>
    public float maxlat { get; private set; }

    /// <summary>
    /// Maximum longitude
    /// </summary>
    public float maxlon { get; private set; }

    /// <summary>
    /// x coordinate
    /// </summary>
    public float x { get; private set; }

    /// <summary>
    /// y coordinate
    /// </summary>
    public float y { get; private set; }

    /// <summary>
    /// Get center
    /// </summary>
    public Vector3 center { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="xmlNode">XML node</param>
    public BoundsFactory(XmlNode xmlNode)
    {
        // Get the values from the node
        minlat = GetAttributes<float>("minlat", xmlNode.Attributes);
        minlon = GetAttributes<float>("minlon", xmlNode.Attributes);
        maxlat = GetAttributes<float>("maxlat", xmlNode.Attributes);
        maxlon = GetAttributes<float>("maxlon", xmlNode.Attributes);

        // Calculate x and y coordinates
        y = (float)((MercatorProjection.latToY(minlat) + MercatorProjection.latToY(maxlat)) / 2);
        x = (float)((MercatorProjection.lonToX(minlon) + MercatorProjection.lonToX(maxlon)) / 2);

        CalculateCenter(x, y);
    }

    // Calculate center
    public void CalculateCenter(float x, float y)
    {
        center = new Vector3(x, 0, y);
    }
}