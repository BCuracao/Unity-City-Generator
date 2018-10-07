using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Environment maker
/// </summary>
internal class EnvironmentFactory : InfrastructureManager
{

    /// <summary>
    /// Green area material to set in import window
    /// </summary>
    private Material greenMat;

    /// <summary>
    /// Parent GameObject
    /// </summary>
    GameObject parent = new GameObject();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="xmlBaseFactory">Instance of XmlBaseFactory</param>
    /// <param name="greenmaterial"> Green area material</param>
    public EnvironmentFactory(XmlBaseFactory xmlBaseFactory, Material greenMaterial) : base(xmlBaseFactory)
    {
        greenMat = greenMaterial;
        CreaterWaterway();
        parent.name = "Environment";
    }

    /**
     * GENERATION OF WATERWAYS IS DISABLED IN THIS VERSION
    //// Calculates the width of the road
    //private static Vector2 Perpendicular(Vector2 v1, Vector2 v2, float length)
    //{
    //    Vector2 v = v2 - v1;
    //    Vector2 p = new Vector2(-v.y, v.x).normalized * length;
    //    return v1 + p;
    //}
    //// Calculates the width of the road
    //private static Vector3 Perpendicular(Vector3 v1, Vector3 v2, float length)
    //{
    //    Vector2 v = Perpendicular(
    //        new Vector2(v1.x, v1.z),
    //        new Vector2(v2.x, v2.z),
    //        length
    //    );
    //    return new Vector3(v.x, (v1.y + v2.y) / 2f, v.y);
    //}
    //// Calculates the vertices of the connected street sections in order make them fit together
    //private static Vector2 Intersection(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2)
    //{
    //    float d = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
    //    if (Mathf.Abs(d) < 1f) return A2 * 0.5f + B2 * 0.5f;
    //    float m = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / d;
    //    return new Vector2(B1.x + (B2.x - B1.x) * m, B1.y + (B2.y - B1.y) * m);
    //}
    //// Calculates the vertices of the connected street sections in order make them fit together
    //private static Vector3 Intersection(Vector3 A1, Vector3 A2, Vector3 B1, Vector3 B2)
    //{
    //    Vector2 v = Intersection(
    //        new Vector2(A1.x, A1.z),
    //        new Vector2(A2.x, A2.z),
    //        new Vector2(B1.x, B1.z),
    //        new Vector2(B2.x, B2.z)
    //    );
    //    return new Vector3(v.x, (A1.y + A2.y + B1.y + B2.y) / 4f, v.y);
    //}

    ///// <summary>
    ///// Nested Class
    ///// </summary>
    //class StreetNode
    //{

    //    public ulong id;
    //    public Vector3 position;
    //    public bool waterway;

    //}
    ///// <summary>
    ///// Nested Class
    ///// Checks the IDs of all WaysFactory objects in the original list to see if they have matchin IDs.
    ///// If they have matching IDs it combines the two seperate sections thats belong to the same street to one object.
    ///// </summary>
    //class Street
    //{

    //    // List of current nodes in the street
    //    public List<StreetNode> nodes = new List<StreetNode>();
    //    public bool looped = false;

    //    // Function to stitch the street to the current list of nodes
    //    public bool Stitch(XmlBaseFactory xmlBaseFactory, WaysFactory street)
    //    {

    //        // Create list to append
    //        List<StreetNode> append = new List<StreetNode>();
    //        for (int i = 0; i < street.ndref.Count; i++)
    //        {
    //            StreetNode node = new StreetNode();
    //            node.id = street.ndref[i];
    //            node.position = xmlBaseFactory.allNodes[node.id] - xmlBaseFactory.boundsFactory.center;
    //            node.waterway = street.isWaterway;
    //            append.Add(node);
    //        }

    //        // Atemmpt to stitch append list to current list of nodes

    //        // If street is empty
    //        if (nodes.Count == 0)
    //        {
    //            nodes = append;
    //        }
    //        // if the first node in the street is the same as the first node in the street we're trying to stich
    //        else if (nodes[0].id == append[0].id)
    //        {
    //            nodes.Reverse();
    //            nodes.AddRange(append);
    //        }
    //        // if the first node in the street is the same as the last node in the street we're trying to stitch
    //        else if (nodes[0].id == append[append.Count - 1].id)
    //        {
    //            append.AddRange(nodes);
    //            nodes = append;
    //        }
    //        // if the last node in the street is the same as the first node in the street we're trying to stitch
    //        else if (nodes[nodes.Count - 1].id == append[0].id)
    //        {
    //            nodes.AddRange(append);
    //        }
    //        // if the last node in the street is the same as the last node in the street we're trying to stitch
    //        else if (nodes[nodes.Count - 1].id == append[append.Count - 1].id)
    //        {
    //            append.Reverse();
    //            nodes.AddRange(append);
    //        }
    //        else
    //        {
    //            return false;
    //        }

    //        // Cleanup data
    //        // Remove duplicate ids
    //        List<StreetNode> copy = new List<StreetNode>();
    //        for (int i = 0; i < nodes.Count; i++)
    //        {
    //            if (copy.Count == 0 || copy[copy.Count - 1].id != nodes[i].id)
    //            {
    //                copy.Add(nodes[i]);
    //            }
    //        }
    //        nodes = copy;

    //        // Remove single waterways
    //        for (int i = 0; i < nodes.Count; i++)
    //        {
    //            bool fPrev = i - 1 >= 0 ? nodes[i - 1].waterway : !nodes[i].waterway;
    //            bool fNext = i + 1 < nodes.Count ? nodes[i + 1].waterway : !nodes[i].waterway;
    //            bool fCurr = nodes[i].waterway;
    //            if (fCurr != fPrev && fCurr != fNext)
    //            {
    //                nodes[i].waterway = !(nodes[i].waterway);
    //            }
    //        }

    //        // Check if waterway is looped
    //        looped = nodes.Count > 2 && nodes[0].id == nodes[nodes.Count - 1].id;

    //        return true;
    //    }

    //    // Once the street has been fully stitched together and we've checked no other street can be added
    //    // we pass it to the CreateWaterway function
    //}

    //// Creates the waterways
    //private void CreateWaterway(Street street, string name)
    //{
    //    List<Vector3> vertices = new List<Vector3>();
    //    List<Vector3> normals = new List<Vector3>();
    //    List<Vector2> uvs = new List<Vector2>();
    //    List<int> triangles0 = new List<int>(); // road
    //    List<int> triangles1 = new List<int>(); // waterway

    //    for (int i = 1; i < street.nodes.Count; i++)
    //    {
    //        StreetNode s0;
    //        StreetNode s1 = street.nodes[i - 1];
    //        StreetNode s2 = street.nodes[i];
    //        StreetNode s3;

    //        // Get point 0
    //        if (i > 1)
    //        {
    //            s0 = street.nodes[i - 2];
    //        }
    //        else if (street.looped)
    //        {
    //            s0 = street.nodes[street.nodes.Count - 1];
    //        }
    //        else
    //        {
    //            s0 = new StreetNode()
    //            {
    //                position = 2 * s1.position - s2.position,
    //                waterway = s1.waterway,
    //            };
    //        }

    //        // Get point 3
    //        if (i < street.nodes.Count - 1)
    //        {
    //            s3 = street.nodes[i + 1];
    //        }
    //        else if (street.looped)
    //        {
    //            s3 = street.nodes[1];
    //        }
    //        else
    //        {
    //            s3 = new StreetNode()
    //            {
    //                position = 2 * s2.position - s1.position,
    //                waterway = s2.waterway,
    //            };
    //        }

    //        // Width of the waterway at each point
    //        float waterWidth = s0.waterway || s1.waterway || s2.waterway || s3.waterway ? riverWid : streamWid;

    //        // Calculate section vertices
    //        Vector3 v1 = Intersection(
    //            Perpendicular(s0.position, s1.position, -waterWidth), Perpendicular(s1.position, s0.position, +waterWidth),
    //            Perpendicular(s2.position, s1.position, +waterWidth), Perpendicular(s1.position, s2.position, -waterWidth)
    //        );

    //        Vector3 v2 = Intersection(
    //            Perpendicular(s0.position, s1.position, +waterWidth), Perpendicular(s1.position, s0.position, -waterWidth),
    //            Perpendicular(s2.position, s1.position, -waterWidth), Perpendicular(s1.position, s2.position, +waterWidth)
    //        );

    //        Vector3 v3 = Intersection(
    //            Perpendicular(s1.position, s2.position, -waterWidth), Perpendicular(s2.position, s1.position, +waterWidth),
    //            Perpendicular(s3.position, s2.position, +waterWidth), Perpendicular(s2.position, s3.position, -waterWidth)
    //        );

    //        Vector3 v4 = Intersection(
    //            Perpendicular(s1.position, s2.position, +waterWidth), Perpendicular(s2.position, s1.position, -waterWidth),
    //            Perpendicular(s3.position, s2.position, -waterWidth), Perpendicular(s2.position, s3.position, +waterWidth)
    //        );

    //        // Add vertices
    //        vertices.Add(v1);
    //        vertices.Add(v2);
    //        vertices.Add(v3);
    //        vertices.Add(v4);

    //        // Add normals
    //        normals.Add(Vector3.up);
    //        normals.Add(Vector3.up);
    //        normals.Add(Vector3.up);
    //        normals.Add(Vector3.up);

    //        // Add uvs
    //        uvs.Add(new Vector2(0, 0));
    //        uvs.Add(new Vector2(1, 0));
    //        uvs.Add(new Vector2(0, 1));
    //        uvs.Add(new Vector2(1, 1));

    //        // Set indices
    //        int p1 = vertices.Count - 4;
    //        int p2 = vertices.Count - 3;
    //        int p3 = vertices.Count - 2;
    //        int p4 = vertices.Count - 1;

    //        // Add triangles
    //        triangles1.Add(p1);
    //        triangles1.Add(p2);
    //        triangles1.Add(p3);
    //        triangles1.Add(p2);
    //        triangles1.Add(p4);
    //        triangles1.Add(p3);

    //    }

    //    // Find mesh center and move vertices
    //    Vector3 center = Vector3.zero;
    //    foreach (Vector3 v in vertices) center += v;
    //    center /= vertices.Count;
    //    for (int i = 0; i < vertices.Count; i++) vertices[i] -= center;

    //    // Create object
    //    GameObject go = new GameObject(name);
    //    MeshFilter mf = go.AddComponent<MeshFilter>();
    //    MeshRenderer mr = go.AddComponent<MeshRenderer>();
    //    MeshCollider mc = go.AddComponent<MeshCollider>();
    //    go.transform.position = center;
    //    mf.mesh.vertices = vertices.ToArray();
    //    mf.mesh.normals = normals.ToArray();
    //    mf.mesh.uv = uvs.ToArray();

    //    // Add triangles & materials
    //    if (triangles0.Count == 0)
    //    {
    //        mf.mesh.triangles = triangles1.ToArray();
    //        mr.material = waterMat;
    //    }
    //    else if (triangles1.Count == 0)
    //    {
    //        mf.mesh.triangles = triangles0.ToArray();
    //        mr.material = waterMat;
    //    }
    //    else
    //    {
    //        mf.mesh.subMeshCount = 2;
    //        mf.mesh.SetTriangles(triangles0.ToArray(), 0);
    //        mf.mesh.SetTriangles(triangles1.ToArray(), 1);

    //        mr.materials = new Material[] { waterMat };
    //    }

    //    // Move object ontop of the terrain
    //    RaycastHit hit;
    //    if (Physics.Raycast(go.transform.position, Vector3.down, out hit))
    //    {
    //        go.transform.position = hit.point + new Vector3(0, 0.035f, 0);
    //    }

    //}

    //// StitchWays takes a list of WaysFactory objects to connect those that belong together. The way it works, is it takes the last street from that list
    //// it then compares it to all other streets in that list if it can be stitched. Two streets can be stitched (conncected without gaps) if they share points with the same id. 
    //// If it finds streets with identical Ids, it combines them in the same Street object, and starts over until there are no more streets to connect.
    //// This is necessary to combine street sections that belong to same street, but are split up in the XML
    //private void StitchWays(List<WaysFactory> queue)
    //{
    //    while (queue.Count > 0)
    //    {
    //        string name = "";
    //        Street street = new Street();
    //        for (int i = queue.Count - 1; i >= 0; i--)
    //        {
    //            // Get last object in the list
    //            WaysFactory item = queue[i];
    //            if (street.Stitch(xmlBaseFactory, item))
    //            {
    //                if (!name.Contains(item.name))
    //                {
    //                    if (name.Length > 0) name += " ";
    //                    name += item.name;
    //                }
    //                // Remove waterway from list if stitched
    //                queue.RemoveAt(i);
    //                i = queue.Count - 1;
    //            }
    //        }
    //        // Check if the way contains at least 3 IDs to avoid misplaced items with no connections
    //        if (street.nodes.Count > 2)
    //        {
    //            CreateWaterway(street, name);
    //        }
    //    }
    //}
    */
    private void MakeGreen(List<WaysFactory> list)
    {
        foreach (WaysFactory green in list)
        {
            GameObject go = new GameObject();
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            go.AddComponent<MeshCollider>();

            mr.material = greenMat;
            go.name = "Green area";

            // Set GameObject as child of parent object
            go.transform.parent = parent.transform;

            RaycastHit hit;

            // Sum gives us the center point of all way nodes
            Vector3 nodeOrigin = xmlBaseFactory.GetOrigin(green);

            // Gets the gameobjects position to place it at the right sport after raycast hit an object
            Vector3 goPosition = nodeOrigin - xmlBaseFactory.boundsFactory.center;

            // Places the gameobject in the middle of the bounds relativ to its local position(nodeOrigin)
            go.transform.position = nodeOrigin - goPosition;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            // Get all ids
            for (int i = 1; i < green.ndref.Count - 1; i++)
            {
                NodeFactory node1 = xmlBaseFactory.allNodes[green.ndref[i - 1]];
                NodeFactory node2 = xmlBaseFactory.allNodes[green.ndref[i]];

                Vector3 v1 = node1 - nodeOrigin;
                Vector3 v2 = node2 - nodeOrigin;

                vertices.Add(v1);
                vertices.Add(v2);

                int p0 = vertices.Count - 2;
                int p1 = vertices.Count - 1;

                triangles.Add(0);
                triangles.Add(p0);
                triangles.Add(p1);

                triangles.Add(0);
                triangles.Add(p1);
                triangles.Add(p0);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 1));


                // Raycast downwards to transform the gameobjects onto terrain.
                if (Physics.Raycast(goPosition, Vector3.down, out hit))
                {
                    //Debug.DrawLine(Vector3.down, hit.point, Color.red);
                    go.transform.position = hit.point + new Vector3(0, 0.030f, 0);
                    //go.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }
            mf.mesh.vertices = vertices.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = triangles.ToArray();
            mf.mesh.uv = uvs.ToArray();
        }
    }

    public void CreaterWaterway()
    { 
        ///* Enable this to generate rivers only */
        //StitchWays(xmlBaseFactory.allWayNodes.FindAll((s) => { return s.isWaterway && s.ndref.Count > 1; }));

        ///* Enable this to generate streams only */
        //StitchWays(xmlBaseFactory.allWayNodes.FindAll((s) => { return s.isStream && s.ndref.Count > 1; }));

        /* Enable this to generate green areas */
        MakeGreen(xmlBaseFactory.allWayNodes.FindAll((s) => { return s.isPark && s.ndref.Count > 1; }));
    }


}
