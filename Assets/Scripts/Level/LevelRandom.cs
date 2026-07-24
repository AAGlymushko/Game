using System;
public class LevelRandom
{
    System.Random random = new System.Random();

    public LevelRandom()
    {
        random = new System.Random();
    }
    public LevelRandom(int seed)
    {
        random = new System.Random(seed);
    }

    public int Next(int min, int max)
    {
        return random.Next(min, max);
    }
}
