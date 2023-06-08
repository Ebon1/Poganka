using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Poganka
{
    public class Poganka : Mod
    {
        public static Mod Instance;
        public static Effect TrailShader;
        public void LoadAssets()
        {
            //sfx

            //fx
            TrailShader = ModContent.Request<Effect>("Poganka/Assets/Effects/TrailShader").Value;
            //tex
            TextureAssets.Item[ItemID.Shotgun] = ModContent.Request<Texture2D>("Poganka/Content/Items/Weapons/Ranged/Shotgun");
            TextureAssets.Item[ItemID.EnchantedSword] = ModContent.Request<Texture2D>("Poganka/Content/Items/Weapons/Melee/EnchantedSword");
        }
        public override void Load()
        {
            Instance = this;
            LoadAssets();
        }
    }
}