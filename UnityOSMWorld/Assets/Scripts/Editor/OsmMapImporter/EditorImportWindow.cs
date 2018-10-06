using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/* Copyright (c) 2018 Sloan Kelly 
 
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software. 
*/

/// <summary>
/// Editor GUI
/// </summary>
public class EditorImportWindow : EditorWindow
{
    /// <summary>
    /// Materials to set in the import window
    /// </summary>
    private Material roadMaterial;
    private Material footwayMaterial;
    private Material roofMaterial;
    private Material buildingMaterial;
    private Material waterMaterial;
    private Material greenMaterial;

    /// <summary>
    /// Values to set in the import window
    /// </summary>
    private float rooftopHeight;
    private float riverWitdh;
    private float streamWidth;

    // Path to OSM file
    private string mapFilePath = "None (Choose OpenMap File)";

    private bool disableUI;
    private bool validFile;
    private bool importing;

    // Creates new menu item
    [MenuItem("Window/Import OpenMap Data")]
    public static void ShowEditorWindow()
    {
        var window = GetWindow<EditorImportWindow>();
        window.titleContent = new GUIContent("Import OpenMap");
        window.Show();
    }

    // Creates the GUI
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(mapFilePath);
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("..."))
        {
            var filePath = EditorUtility.OpenFilePanel("Select OpenMap File", Application.dataPath, "xml");
            if (filePath.Length > 0)
            {
                mapFilePath = filePath;
            }
            validFile = mapFilePath.Length > 0;
        }

        EditorGUILayout.EndHorizontal();
        
        // Fields for the import options
        roadMaterial = EditorGUILayout.ObjectField("Road Material", roadMaterial, typeof(Material), false) as Material;
        footwayMaterial = EditorGUILayout.ObjectField("Footway Material", footwayMaterial, typeof(Material), false) as Material;
        buildingMaterial = EditorGUILayout.ObjectField("Building Material", buildingMaterial, typeof(Material), false) as Material;
        waterMaterial = EditorGUILayout.ObjectField("Waterway Material", waterMaterial, typeof(Material), false) as Material;
        roofMaterial = EditorGUILayout.ObjectField("Roof Material", roofMaterial, typeof(Material), false) as Material;
        greenMaterial = EditorGUILayout.ObjectField("Green area Material", greenMaterial, typeof(Material), false) as Material;
        rooftopHeight = EditorGUILayout.FloatField("Rooftop Height", rooftopHeight);
        riverWitdh = EditorGUILayout.FloatField("River Width", riverWitdh);
        streamWidth = EditorGUILayout.FloatField("Stream Width", streamWidth);


        EditorGUI.BeginDisabledGroup(!validFile || disableUI);
        if (GUILayout.Button("Import Map File"))
        {

            var mapWrapper = new ImportMapWrapper(this, mapFilePath, roadMaterial, footwayMaterial, buildingMaterial,
                                                        waterMaterial, roofMaterial, greenMaterial, rooftopHeight, riverWitdh, streamWidth);

            mapWrapper.Import();
        }
        EditorGUI.EndDisabledGroup();

        if (disableUI)
        {
            EditorGUILayout.HelpBox(" The current scene has not been saved yet!", MessageType.Warning, true);
        } 
    }

    private void Update()
    {
        // Disable the import function is there are unsaved changes in the scene
        disableUI = EditorSceneManager.GetActiveScene().isDirty;
    }
}
