using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// it isn't intended to be used as a fully-functional factory.
// So mark it as abstract, which makes it impossible to create object instances of it.
public abstract class GameObjectFactory : ScriptableObject
{
    Scene scene;

    // it is only accessible to the class itself and all types that extend it.
    protected T CreateGameObjectInstance<T> (T prefab) where T : MonoBehaviour
    {
        if (!scene.isLoaded)
        {
            if (Application.isEditor)
            {
                scene = SceneManager.GetSceneByName(name);
                if (!scene.isLoaded)
                {
                    scene = SceneManager.CreateScene(name);
                }
            } else
            {
                scene = SceneManager.CreateScene(name);
            }
        }

        T instance = Instantiate(prefab);
        SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
        return instance;
    }
}
