using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Transform ground = default;


    Vector2Int size;

    public void Initialize(Vector2Int size)
    {
        this.size = size;
        ground.localScale = new Vector3(size.x, size.y, 1f);
    }
}
