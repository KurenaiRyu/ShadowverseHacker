using UnityEngine;

namespace ShadowverseHacker
{
    public class Loader
    {
        public static void Init()
        {
            if (Load != null)
            {
                Object.Destroy(Load);                
            }
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<Hacker>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void Unload()
        {
            Object.DestroyImmediate(Load);
        }

        private static GameObject Load;
    }
}
