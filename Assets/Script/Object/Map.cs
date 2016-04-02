using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Map {
    public int row_size;
    public int col_size;
    int[][] block_id_map;
    int[][] height_map;
    public Map(int row_size,int col_size)
    {
        this.row_size = row_size;
        this.col_size = col_size;
        block_id_map = new int[row_size][];
        height_map = new int[row_size][];
        for (int i=0;i < row_size;i++)
        {
            height_map[i] = new int[col_size];
            block_id_map[i] = new int[col_size];
            for (int j = 0; j < col_size; j++)
            {
                block_id_map[i][j] = 0;
                height_map[i][j] = 0;
            }
        }
    }
    public static void ResizeMap(int new_row_size, int new_col_size,Map map)
    {
        Map new_map = new Map(new_row_size, new_col_size);
        for (int i = 0; i < Math.Max(map.row_size,new_row_size); i++)
        {
            for (int j = 0; j < Math.Max(map.col_size,new_col_size); j++)
            {
                new_map.block_id_map[i][j] = map.block_id_map[i][j] = 0;
                new_map.height_map[i][j] = map.height_map[i][j] = 0;
            }
        }
        map = new_map;
    }
}
