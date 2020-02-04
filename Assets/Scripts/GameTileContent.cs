using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{

    [SerializeField]
    GameTileContentType type = default;

    public GameTileContentType Type => type;

    public bool BlockPath =>
        Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;

    GameTileContentFactory originFactory;

    public GameTileContentFactory OriginFactory
    {
        get => originFactory;

        set
        {
            Debug.Assert(originFactory == null, "Redefine origin factory");
            originFactory = value;
        }
    }


    public void Recycle()
    {
        originFactory.Reclaim(this);
    }
}
