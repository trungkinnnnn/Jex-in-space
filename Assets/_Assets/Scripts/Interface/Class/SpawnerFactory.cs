public static class SpawnerFactory
{
    private static ILeanPoolSpawner _instance;

    public static ILeanPoolSpawner GetSpawner()
    {
        if (_instance == null)
        {
            _instance = new LeanPoolSpawner();  
        }
        return _instance;
    }
}