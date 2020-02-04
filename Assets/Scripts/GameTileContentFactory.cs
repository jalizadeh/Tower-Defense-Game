using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{

    [SerializeField]
    GameTileContent destinationPrefab = default;

    [SerializeField]
    GameTileContent emptyPrefab = default;

    [SerializeField]
    GameTileContent wallPrefab = default;

    [SerializeField]
    GameTileContent spawnpointPrefab = default;

    [SerializeField]
    GameTileContent towerPrefab = default;


    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong Facotry Reclaimed");
        Destroy(content.gameObject);
    }

    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public GameTileContent Get (GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyPrefab);

            case GameTileContentType.Destination:
                return Get(destinationPrefab);

            case GameTileContentType.Wall:
                return Get(wallPrefab);

            case GameTileContentType.SpawnPoint:
                return Get(spawnpointPrefab);

            case GameTileContentType.Tower:
                return Get(towerPrefab);
        }

        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }

}
