using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rooftop maker
/// </summary>
internal class RoofFactory : InfrastructureManager
{
    /// <summary>
    /// Roof material
    /// </summary>
    private Material roofMat;

    /// <summary>
    /// Roof height
    /// </summary>
    private float roofHeight;

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
    /// Parent GameObject
    /// </summary>
    GameObject parent = new GameObject();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="xmlBaseFactory">Instance of XmlBaseFactory</param>
    /// <param name="roofMaterial">Roof material</param>
    /// <param name="rooftopHeight">Height of rooftop</param>
    public RoofFactory(XmlBaseFactory xmlBaseFactory, Material roofMaterial, float rooftopHeight) : base(xmlBaseFactory)
    {
        roofMat = roofMaterial;
        roofHeight = rooftopHeight;
        CreateRooftops();
        parent.name = "Rooftops";
    }

    // Create rooftops
    private void CreateRooftops()
    {
        foreach (var building in xmlBaseFactory.allWayNodes.FindAll((b) => { return b.isBuilding && b.ndref.Count > 3; }))
        {
            GameObject go = new GameObject();
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            // Material for the roof
            mr.material = roofMat;

            /* un-comment to enable colliders for the rooftops */
            go.AddComponent<MeshCollider>();

            /* Hide Roof objects in the project hierarchy to make it more readable */
            // go.hideFlags = HideFlags.HideInHierarchy;

            // Set GameObject as child of parent object
            go.transform.parent = parent.transform;

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

                // Calculate rooftop
                Vector3 rooftop = new Vector3(0, building.height + roofHeight, 0); ;
                vertices.Add(rooftop);
                uvs.Add(new Vector2(0.5f, 0.5f));
                normals.Add(Vector3.up);

                // Calculate vertices
                Vector3 v1 = (node1 - nodeOrigin);
                Vector3 v2 = (node2 - nodeOrigin);
                Vector3 v3 = v1 + new Vector3(0, building.height + 1, 0);
                Vector3 v4 = v2 + new Vector3(0, building.height + 1, 0);

                // Add vertices
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                vertices.Add(v4);

                // Points to form the triangles
                int p1 = vertices.Count - 4;
                int p2 = vertices.Count - 3;
                int p3 = vertices.Count - 2;
                int p4 = vertices.Count - 1;

                // Indices of the triangle
                triangles.Add(0);
                triangles.Add(p3);
                triangles.Add(p4);

                // Indices of the triangle
                triangles.Add(p4);
                triangles.Add(p3);
                triangles.Add(0);

                // Add Normals
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);

                // Add UVs
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));


            }

            mf.mesh.vertices = vertices.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = triangles.ToArray();
            mf.mesh.uv = uvs.ToArray();

            // Raycasts downwards and places it on the first surface it hits
            if (Physics.Raycast(goPosition, Vector3.down, out hit))
            {
                // Debug.DrawLine(Vector3.down, hit.point, Color.red);
                go.transform.position = hit.point - new Vector3(0, 2, 0);
                // go.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }

        }
        // Clear data
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();
    }
}