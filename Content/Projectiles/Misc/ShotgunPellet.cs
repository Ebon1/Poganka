using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
namespace Poganka.Content.Projectiles.Misc
{
    public class ShotgunPellet : ModProjectile
    {
        public override string Texture => "Poganka/Assets/Tex/Extras/wideBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = 1;
        }
        public override void AI()
        {
            if (Projectile.velocity.Length() < 0.3f && Projectile.velocity.Length() > -0.3f)
            {
                if (Projectile.ai[0] <= 0)
                    Projectile.Kill();
                Projectile.ai[0] -= 0.1f;
            }
            Projectile.velocity *= 0.96f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            Main.spriteBatch.Reload(BlendState.Additive);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition, null, Color.Orange * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, (1f - fadeMult * i) * 0.4f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition, null, Color.White * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, (1f - fadeMult * i) * 0.4f, SpriteEffects.None, 0);
                if (i == 0) continue;
                Vector2 start = Projectile.oldPos[i];
                while (Projectile.oldPos[i - 1].Distance(start) > 1)
                {
                    start += Helper.FromAToB(start, Projectile.oldPos[i - 1]);
                    Main.spriteBatch.Draw(tex, start - Main.screenPosition, null, Color.Orange * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, (1f - fadeMult * i) * 0.4f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex, start - Main.screenPosition, null, Color.White * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, (1f - fadeMult * i) * 0.4f, SpriteEffects.None, 0);
                }
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
