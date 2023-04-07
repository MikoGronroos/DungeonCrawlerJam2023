using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public static Dictionary<Vector2, GridCell> GridCells { get; set; } = new Dictionary<Vector2, GridCell>();

    private void Start()
    {
        GridCells.Clear();
        foreach (var item in FindObjectsOfType<GridCell>())
        {
            GridCells.Add(new Vector2(item.x, item.z), item);
        }
    }
}
