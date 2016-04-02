using UnityEngine;
using UnityEditor;
using System.Collections;

public class CustomEditorMenu : EditorWindow {

	[MenuItem ("My Tools/Map Editor",false,0)]
    static void MapEditor()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
    }
}
