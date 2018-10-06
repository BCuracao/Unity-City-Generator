using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Infrastructure base
/// </summary>
internal abstract class InfrastructureManager
{
    /// <summary>
    /// Reference to XmlBaseFactory object
    /// </summary>
    protected XmlBaseFactory xmlBaseFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    public InfrastructureManager(XmlBaseFactory map)
    {
        xmlBaseFactory = map;
    }

    //// Calculates the center of a way
    //protected Vector3 GetCenter(WaysFactory waysFactory)
    //{
    //    Vector3 sum = Vector3.zero;

    //    foreach (var w in waysFactory.ndref)
    //    {
    //        sum += xmlBaseFactory.allNodes[w];
    //    }

    //    return sum / waysFactory.ndref.Count;
    //}

}