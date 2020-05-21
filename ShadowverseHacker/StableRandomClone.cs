using System;
using System.Reflection;

namespace ShadowverseHacker
{
    class StableRandomClone
    {
        private Random random;

        public StableRandomClone()
        {
            CreateRandom(typeof(BattleMgrBase)
                .GetField("_stableRandom", 
                    BindingFlags.Instance | BindingFlags.NonPublic)?
                .GetValue(BattleMgrBase.GetIns()) as Random
            );
        }

        private void CreateRandom(Random originalRandom)
        {
            Random cloneRandom = new Random();
            Type typeFromHandle = typeof(Random);
            FieldInfo field = typeFromHandle.GetField("inext", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(cloneRandom, field.GetValue(originalRandom));
            field = typeFromHandle.GetField("inextp", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(cloneRandom, field.GetValue(originalRandom));
            field = typeFromHandle.GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
            int[] array = (int[])field.GetValue(originalRandom);
            field.SetValue(cloneRandom, array.Clone());
            random = cloneRandom;
        }

        public int StableRandom(int val)
        {
            double randomResult = random.NextDouble();
            return (int)Math.Floor(val * randomResult);
        }
    }
}
