using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    public readonly float unit_distance = Mathf.Sqrt(0.5f);
    private Vector3 origin;
    private Vector3 unit_width_x;
    private Vector3 unit_width_y;
    
    private int cursor_type;
    public const int CURSOR_TYPE_BLOCK = 0;
    public const int CURSOR_TYPE_RECT = 1;
    private Vector3 cursor_position;

    [HideInInspector]
    public float width = Mathf.Sqrt(0.5f);
    [HideInInspector]
    public float height = Mathf.Sqrt(0.5f);
    [HideInInspector]
    public float level_height = 1.0f;
    [HideInInspector]
    public int level = 0;
    [HideInInspector]
    public int row_size = 100;
    [HideInInspector]
    public int col_size = 100;
    [HideInInspector]
    public bool is_display = true;
    [HideInInspector]
    

    public struct Locator
    {
        public readonly int x, y;
        public Locator(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Locator(Vector2 pos_on_grid, Grid grid)
        {
            this.x = Mathf.CeilToInt((pos_on_grid.x + pos_on_grid.y - grid.unit_distance) / (grid.unit_distance * 2));
            this.y = Mathf.CeilToInt((pos_on_grid.y - pos_on_grid.x - grid.unit_distance) / (grid.unit_distance * 2));
        }
    }

    public Grid() : base()
    {
        SetDefault();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (is_display)
            DrawMapGrid();
        Gizmos.color = Color.red;
        switch(cursor_type)
        {
            case CURSOR_TYPE_BLOCK:
                DrawBlockCursorGrid();
                break;
            case CURSOR_TYPE_RECT:
                break;
        }     
    }
    private void DrawMapGrid()
    {
        if (true)
        {
            //橫線
            for (int i = 0; i <= row_size; i++)
            {
                Vector3 start = origin + unit_width_y * i;
                Vector3 end = origin + col_size * unit_width_x + unit_width_y * i;
                start.y = 1.0f * level;
                end.y = 1.0f * level;
                Gizmos.DrawLine(start, end);
            }
            //直線
            for (int i = 0; i <= col_size; i++)
            {
                Vector3 start = origin + unit_width_x * i;
                Vector3 end = origin + row_size * unit_width_y + unit_width_x * i;
                start.y = 1.0f * level;
                end.y = 1.0f * level;
                Gizmos.DrawLine(start, end);
            }
        }
    }
    private void DrawBlockCursorGrid()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f));
        Gizmos.matrix = Matrix4x4.TRS(cursor_position, rotation, new Vector3(1.0f, 1.0f, 1.0f));
        Gizmos.DrawWireCube(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f));
    }
    private void DrawRectAreaGrid()
    {

    }
    public void SetCursorPosition(Locator locator)
    {
        cursor_position = FindBlockPivotByLocator(locator);
    }
    public Vector3 FindBlockPivotByLocator(Locator locator)
    {
        Vector3 position = (new Vector3(unit_distance, 0.0f, unit_distance) * locator.x +
                            new Vector3(-unit_distance, 0.0f, unit_distance) * locator.y);
        position.y = level_height * level;
        return position;
    }
    public void SetToolCursor(int type_id)
    {
        cursor_type = type_id;
    }
    public void SetDefault()
    {
        width = unit_distance;
        height = unit_distance;
        origin = new Vector3(0.0f, 1.0f * level, -unit_distance);
        unit_width_x = new Vector3(-width, 0.0f, width);
        unit_width_y = new Vector3(height, 0.0f, height);
    }
}
