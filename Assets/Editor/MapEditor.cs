using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapEditor : EditorWindow {
    GameObject block_instance;
    //Grid使用變數
    private static Grid grid;
    
    //編輯器本身變數
    private Ray mouseRay;
    bool grid_flodout = true;
    //GUI Grid Fold內用變數
    bool brushTools_flodout = true;
    private int[] edit_area_size;
    
    //Brush Tool Bar
    public struct DrawTool
    {
        public static int tool;
        public static string[] tool_names = { "Brush", "Rect", "Erease" };
        public const int BRUSH_TOOL = 0;
        public const int RECT_TOOL = 1;
        public const int EREASE_TOOL = 2;
    }
    public void OnEnable()
    {
        block_instance = Resources.Load<GameObject>("Prefabs/block");
        grid = GetGrid();
        GetMapObj();
        SceneView.onSceneGUIDelegate += OnSecneGUI;
    }
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Map Editor ver0.20");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Map Path(JSON)");
        GUILayout.EndHorizontal();
        
        if (brushTools_flodout = EditorGUILayout.Foldout(brushTools_flodout, "Brush Tools"))
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                DrawTool.tool = GUILayout.Toolbar(DrawTool.tool, DrawTool.tool_names);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear Map"))
                {
                    ClearMap();
                }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        if (grid_flodout = EditorGUILayout.Foldout(grid_flodout, "Grid Helper"))
        {
            if (grid == null)
            {
                grid = GetGrid();
            }
            grid.is_display = GUILayout.Toggle(grid.is_display, "Grid Visible");
            grid.level = EditorGUILayout.IntSlider("Grid Level", grid.level, 0, 50);
            grid.row_size = EditorGUILayout.IntField("Grid Row Size", grid.row_size);
            grid.col_size = EditorGUILayout.IntField("Grid Column Size", grid.col_size);
        }
        GUILayout.EndVertical();
    }

    /** @brief 一個插入SceneView中的指派。
     * 函式形式插入SceneView的指派 void Function (SceneView sceneview)，為固定形式
     * 使用滑鼠點在SceneView上位置產生對應到世界座標的射線(RAY)，在射線與格線(Grid)所在平面相交之位置上產生相應方塊。
     * 格線平面位置為y = k，k為目前設置之格線所在高度，均為正整數。
     * */
    public void OnSecneGUI(SceneView sceneview)
    {
        CreateBlockOnScene();
    }

    /**@brief 取得滑鼠射線與平面焦點
     * 取得滑鼠射線與平面焦點
     * 對於求RAY與XZ平面焦點的校正，高度會與距離成正比
     * 但是從GUI的點上指向要擺東西的平面(在此為XZ平面)，Y指向會是負(即向下指)，因此算出來距離會是負，要對DISTANCE做負校正，level則是對XZ平面做高度調整*/
    
    public void CreateBlockOnScene()
    {
        Event e = Event.current;
        Grid.Locator locator = LocateMouseOnGrid(e.mousePosition);
        grid.SetCursorPosition(locator);
        //右鍵實例化方塊Prefab
        if (!e.alt)
        {
            if ((e.type == EventType.MouseDown  || e.type == EventType.MouseDrag)  && e.button == 1)
            {
                e.Use();
                if (DrawTool.tool == DrawTool.BRUSH_TOOL)
                {
                    BrushDraw(locator);
                }
            }
        }
    }
    private void BrushDraw(Grid.Locator locator)
    {
        Vector3 block_position = grid.FindBlockPivotByLocator(locator);
        if (block_instance)
        {
            string block_name = string.Format("block[{0},{1}]", locator.x, locator.y);
            GameObject clone;
            if ((clone = GameObject.Find(block_name)) != null)
            {
                Object[] undo_obj = { clone.transform, clone.GetComponent<BlockInfo>() };
                //註冊Undo事件，改變高度
                Undo.RecordObjects(undo_obj, "Set " + clone.name + " to level" + block_position.y);
                clone.transform.position = block_position;
            }
            else
            {
                clone = PrefabUtility.InstantiatePrefab(block_instance) as GameObject;
                clone.name = block_name;
                clone.transform.position = block_position;
                //註冊Undo事件，創造方塊
                Undo.RegisterCreatedObjectUndo(clone, "Create" + clone.name);
            }
            clone.GetComponent<BlockInfo>().SetPosition(locator.x, locator.y, grid.level);
            clone.transform.parent = GetMapObj().transform;
        }
    }

    private void AreaBatchDraw(Grid.Locator start, Grid.Locator end)
    {

    }

    private Grid.Locator LocateMouseOnGrid(Vector2 mouse_position)
    {
        //將滑鼠雷射指向Grid平面
        mouseRay = HandleUtility.GUIPointToWorldRay(mouse_position);
        float distance = -((mouseRay.origin.y - grid.level_height * grid.level) / (mouseRay.direction.y));
        Vector3 position = mouseRay.origin + distance * mouseRay.direction;
        //對位置做計算，將其轉換成在Grid平面上的(X,Y)座標
        Grid.Locator location = new Grid.Locator(new Vector2(position.x, position.z), grid.unit_distance);
        //在Scene上顯示目前滑鼠所在位置資訊
        Handles.BeginGUI();
        GUI.contentColor = Color.white;
        GUIStyle labels_style = new GUIStyle();
        labels_style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 0, 400, 20), string.Format("Mouse on Block({0},{1})", location.x, location.y), labels_style);
        Handles.EndGUI();
        return location;
    }
    private void ClearMap()
    {
        GameObject map_obj = GetMapObj();
        for (int i = map_obj.transform.childCount - 1 ; i >=0 ;i--)
        {
           DestroyImmediate(map_obj.transform.GetChild(i).gameObject);
        }
    }
    private static Grid GetGrid()
    {
        if (GameObject.Find("Map Grid") != null)
        {
            return GameObject.Find("Map Grid").GetComponent<Grid>();
        }
        else
        {
            return new GameObject("Map Grid").AddComponent<Grid>();
        }
    }
    private static GameObject GetMapObj()
    {
        if (GameObject.Find("Map") != null)
        {
            return GameObject.Find("Map");
        }
        else
        {
            return new GameObject("Map"); ;
        }
    }
}
