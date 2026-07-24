using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    LevelRandom levelRandom = new LevelRandom();
    List<GameObject> fieldObjects = new List<GameObject>();
    LevelGenerator levelGenerator = new LevelGenerator();

    public void generate()
    {
        levelGenerator.generate(levelRandom, fieldObjects);
    }

    void Start()
    {
        
    }
    void Update()
    {

    }
}
