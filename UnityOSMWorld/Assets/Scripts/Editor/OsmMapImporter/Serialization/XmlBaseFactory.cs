using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// OSM file reader
/// </summary>
internal sealed class XmlBaseFactory
{
    /// <summary>
    /// File to read
    /// </summary>
    public string resourceFile;

    /// <summary>
    /// Reference to BoundsFactory
    /// </summary>
    public BoundsFactory boundsFactory;

    /// <summary>
    /// List of all ways
    /// </summary>
    public List<WaysFactory> allWayNodes;

    /// <summary>
    /// Dictionary with all nodes and ids
    /// </summary>
    public Dictionary<ulong, NodeFactory> allNodes;


    // Load the OsmMap data resource file.
    public void Read(string resourceFile)
    {
        // Initialization of data structures                                                                                                                           
        allWayNodes = new List<WaysFactory>();
        allNodes = new Dictionary<ulong, NodeFactory>();

        // Loading the inputData as TextAsset and converting it to an xml document
        var xmlFile = File.ReadAllText(resourceFile);
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xmlFile);

        // Pass contents of the xml file
        GetBounds(xmldoc.SelectSingleNode("/osm/bounds"));
        GetNodes(xmldoc.SelectNodes("/osm/node"));
        GetWays(xmldoc.SelectNodes("/osm/way"));
    }

    // Passes all ways to retrieve a list of all way objects
    private void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            WaysFactory waysFactory = new WaysFactory(xmlNode);
            // List of objects of type WaysFactory
            allWayNodes.Add(waysFactory);
            // Center of all points that belong to a way
            GetOrigin(waysFactory);
        }
    }
    // Passes the bounds to retrieve the boundaries of the data
    private void GetBounds(XmlNode xmlNode)
    {
        boundsFactory = new BoundsFactory(xmlNode);
    }
    // Passes all nodes to retrieve a list of all node objects
    private void GetNodes(XmlNodeList xmlNodeList)
    {
        //Pass the contents one by one to factory class
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            NodeFactory nodeFactory = new NodeFactory(xmlNode);
            allNodes.Add(nodeFactory.id, nodeFactory);
        }
    }

    // Calculates the center point of all ids that belong to a way4
    // Returns the local origin
    public Vector3 GetOrigin(WaysFactory allPoints)
    {
        Vector3 sum = new Vector3(0, 0, 0);

        foreach (var w in allPoints.ndref)
        {
            sum += allNodes[w];
        }
        return sum / allPoints.ndref.Count;
    }
}