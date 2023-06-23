using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Poganka.Helper;
using Microsoft.Xna.Framework;
using Poganka.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace Poganka.Content.Projectiles.Weapons.Melee
{
    public class IceBladeP1 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.scale = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height), new Vector2(Projectile.scale, MathHelper.Clamp(Projectile.scale * 2, 0, 1)), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int num495 = 0; num495 < 15; num495++)
            {
                int num496 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height * 2, 92, Projectile.velocity.X, Projectile.velocity.Y, Main.rand.Next(0, 101), default(Color), 1f + (float)Main.rand.Next(40) * 0.01f);
                Main.dust[num496].noGravity = true;
                Dust dust2 = Main.dust[num496];
                dust2.velocity *= 2f;
            }
        }
        public override bool ShouldUpdatePosition() => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.scale < 1)
                Projectile.scale += 0.1f;
            if (Projectile.timeLeft == 35)
            {
                SoundStyle style = SoundID.Item30;
                style.Volume = 0.5f;
                SoundEngine.PlaySound(style, Projectile.Center);

                if (Projectile.ai[0] < 2)
                {
                    Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 20 * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 100, Vector2.UnitY, 500, true)), Projectile.velocity, ModContent.ProjectileType<IceBladeP1>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
                    a.ai[0] = Projectile.ai[0] + 1;
                }
                else
                {
                    Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 20 * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 100, Vector2.UnitY, 500, true)), Projectile.velocity, ModContent.ProjectileType<IceBladeP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
                    a.ai[0] = 0;
                }
            }
            Projectile.velocity.Normalize();
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
            Projectile.rotation = MathHelper.ToRadians(20 * Projectile.direction);
        }
    }
    public class IceBladeP2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 72;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.scale = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height), new Vector2(Projectile.scale, MathHelper.Clamp(Projectile.scale * 2, 0, 1)), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int num495 = 0; num495 < 15; num495++)
            {
                int num496 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height * 2, 92, Projectile.velocity.X, Projectile.velocity.Y, Main.rand.Next(0, 101), default(Color), 1f + (float)Main.rand.Next(40) * 0.01f);
                Main.dust[num496].noGravity = true;
                Dust dust2 = Main.dust[num496];
                dust2.velocity *= 2f;
            }
        }
        public override bool ShouldUpdatePosition() => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.scale < 1)
                Projectile.scale += 0.1f;
            if (Projectile.timeLeft == 35)
            {
                SoundStyle style = SoundID.Item30;
                style.Volume = 0.5f;
                SoundEngine.PlaySound(style, Projectile.Center);
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 24 * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 100, Vector2.UnitY, 500, true)), Projectile.velocity, ModContent.ProjectileType<IceBladeP3>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
                a.ai[0] = 0;
            }
            Projectile.velocity.Normalize();
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
            Projectile.rotation = MathHelper.ToRadians(20 * Projectile.direction);
        }
    }
    public class IceBladeP3 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 72;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.scale = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height), new Vector2(Projectile.scale, MathHelper.Clamp(Projectile.scale * 2, 0, 1)), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int num495 = 0; num495 < 15; num495++)
            {
                int num496 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height * 2, 92, Projectile.velocity.X, Projectile.velocity.Y, Main.rand.Next(0, 101), default(Color), 1f + (float)Main.rand.Next(40) * 0.01f);
                Main.dust[num496].noGravity = true;
                Dust dust2 = Main.dust[num496];
                dust2.velocity *= 2f;
            }
        }
        public override bool ShouldUpdatePosition() => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.scale < 1)
                Projectile.scale += 0.1f;
            Projectile.velocity.Normalize();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
            if (Projectile.ai[1] == 0)
                Projectile.rotation = MathHelper.ToRadians(20 * Projectile.direction);
            else
            {
                NPC npc = Main.npc[(int)Projectile.ai[2]];
                if (npc.active)
                    Projectile.Center = npc.Center;
            }
        }
    }
}
