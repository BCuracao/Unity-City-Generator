using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building maker
/// </summary>
internal class BuildingFactory : InfrastructureManager
{
    /// <summary>
    /// Building material to set in import window
    /// </summary>
    private Material buildingMat;

    GameObject parent = new GameObject();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="xmlBaseFactory">Instance of XmlBaseFactory</param>
    /// <param name="buildingMaterial">Road material</param>
    public BuildingFactory(XmlBaseFactory xmlBaseFactory, Material buildingMaterial) : base(xmlBaseFactory)
    {
        buildingMat = buildingMaterial;
        CreateBuildings();
        parent.name = "Buildings";
    }

    /// <summary>
    /// List of vertices
    /// </summary>
    private List<Vector3> vertices;

    /// <summary>
    /// List of normals
    /// </summary>
    private List<Vector3> normals;

    /// <summary>
    /// List of uvs
    /// </summary>
    private List<Vector2> uvs;

    /// <summary>
    /// List of triangles
    /// </summary>
    private List<int> triangles;

    /// <summary>
    /// GameObject
    /// </summary>
    private GameObject go;


    // Create buildings
    public void CreateBuildings()
    {
        // Iterate through all ways that are buildings
        foreach (var building in xmlBaseFactory.allWayNodes.FindAll((b) => { return b.isBuilding && b.ndref.Count > 1; }))
        {
            go = new GameObject();
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshCollider>();

            // Set building name
            go.name = building.name;

            // Set GameObject as child of parent object
            go.transform.parent = parent.transform;

            // set building mat
            mr.material = buildingMat;

            // Sum gives us the center point of all way nodes
            Vector3 nodeOrigin = xmlBaseFactory.GetOrigin(building);

            // Gets the gameobjects position to place it at the right sport after raycast hit an object
            Vector3 goPosition = nodeOrigin - xmlBaseFactory.boundsFactory.center;

            // Places the gameobject in the middle of the bounds relativ to its local position(nodeOrigin)
            go.transform.position = nodeOrigin - goPosition;

            // Raycast to place all buildings on top of mesh
            RaycastHit hit;

            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uvs = new List<Vector2>();
            triangles = new List<int>();

            for (int i = 1; i < building.ndref.Count; i++)
            {
                // Get the first two nodes that belong to the way
                NodeFactory node1 = xmlBaseFactory.allNodes[building.ndref[i - 1]];
                NodeFactory node2 = xmlBaseFactory.allNodes[building.ndref[i]];

                // Use x and y coordinates of both nodes to
                Vector3 v1 = (node1 - nodeOrigin);
                Vector3 v2 = (node2 - nodeOrigin);

                Vector3 v3 = v1 + new Vector3(0, building.height, 0);
                Vector3 v4 = v2 + new Vector3(0, building.height, 0);

                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                vertices.Add(v4);

                Color c = Color.red;
                Debug.DrawLine(v1, v2, c);
                

                // Points to form the triangles needed for the plane
                int p1 = vertices.Count - 4;
                int p2 = vertices.Count - 3;
                int p3 = vertices.Count - 2;
                int p4 = vertices.Count - 1;

                // Triangle for the mesh (front side)
                triangles.Add(p1);
                triangles.Add(p3);
                triangles.Add(p2);

                // Triangle for the mesh (front side)
                triangles.Add(p3);
                triangles.Add(p4);
                triangles.Add(p2);

                // Triangle for the mesh (back side)
                triangles.Add(p2);
                triangles.Add(p3);
                triangles.Add(p1);

                // Triangle for the mesh (back side)
                triangles.Add(p2);
                triangles.Add(p4);
                triangles.Add(p3);

                // Normals
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);

                // UVs
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));

                // Raycast downwards to transform the gameobjects onto terrain.
                if (Physics.Raycast(goPosition, Vector3.down, out hit))
                {
                    //Debug.DrawLine(Vector3.down, hit.point, Color.red);
                    go.transform.position = hit.point - new Vector3(0, 0.5f, 0);
                    //go.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }

            mf.mesh.vertices = vertices.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = triangles.ToArray();
            mf.mesh.uv = uvs.ToArray();
        }
        // Clear data
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();
    }
}