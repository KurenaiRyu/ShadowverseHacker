using UnityEngine;

namespace ShadowverseHacker
{
    public class Loader
    {
        public static void Load()
        {
            if (loadGameObject != null)
            {
                UnityEngine.Object.Destroy(loadGameObject);                
            }
            Loader.loadGameObject = new GameObject();
            Loader.loadGameObject.AddComponent<Hacker>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.loadGameObject);
        }

        public static void Unload()
        {
            UnityEngine.Object.DestroyImmediate(loadGameObject);
        }

        private static GameObject loadGameObject;
    }
}
