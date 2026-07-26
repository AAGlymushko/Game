using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    LevelRandom levelRandom = null;
    List<GameObject> fieldObjects = null;
    LevelGenerator levelGenerator = null;

    private void Awake()
    {
        levelRandom = new LevelRandom();
        fieldObjects = new List<GameObject>();
        levelGenerator = new LevelGenerator();
    }

    public void clear()
    {
        if (fieldObjects == null) return;
        foreach (GameObject gameObject in fieldObjects)
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        fieldObjects.Clear();
    }

    public void generate()
    {
        clear();
        if (levelGenerator == null) levelGenerator = new LevelGenerator();
        if (levelRandom == null) levelRandom = new LevelRandom();
        if (fieldObjects == null) fieldObjects = new List<GameObject>();
        levelGenerator.generate(levelRandom, fieldObjects);
    }

}
