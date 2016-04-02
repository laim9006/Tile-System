using UnityEngine;
using System.Collections;

public class BlockInfo : MonoBehaviour {
    public int row = 0;
    public int col = 0;
    public int level = 0;

	public void SetPosition(int rol,int col,int level)
    {
        this.row = rol;
        this.col = col;
        this.level = level;
    }
    public int GetRow() { return this.row; }
    public int GetCol() { return this.col; }
}
