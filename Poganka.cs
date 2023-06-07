using Terraria.ModLoader;

namespace Poganka
{
    public class Poganka : Mod
    {
        public static Mod Instance;
        public override void Load()
        {
            Instance = this;
        }
    }
}