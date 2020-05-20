using System;
using System.Reflection;

namespace ShadowverseHacker
{
    class StableRandomClone
    {
        private Random random;

        public StableRandomClone()
        {
            Random random = typeof(BattleMgrBase).GetField("_stableRandom", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(BattleMgrBase.GetIns()) as System.Random;
            CreateRandom(random);
        }

        private void CreateRandom(Random rand)
        {
            Random obj = new Random();
            Type typeFromHandle = typeof(Random);
            FieldInfo field = typeFromHandle.GetField("inext", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(obj, field.GetValue(rand));
            field = typeFromHandle.GetField("inextp", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(obj, field.GetValue(rand));
            field = typeFromHandle.GetField("SeedArray", BindingFlags.Instance | BindingFlags.NonPublic);
            int[] array = (int[])field.GetValue(rand);
            field.SetValue(obj, array.Clone());
            this.random = obj;
        }

        public int StableRandom(int val)
        {
            double randomResult = this.random.NextDouble();
            return (int)Math.Floor((double)val * randomResult);
        }
    }
}
