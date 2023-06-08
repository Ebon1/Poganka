using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Poganka.Content.Projectiles.Weapons;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static Humanizer.In;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Poganka.Content.Projectiles.Weapons.Melee
{
    public class EnchantedSwordP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TerraBlade2);
            Projectile.aiStyle = -1;
            Projectile.Size = new Vector2(30, 72);
            Projectile.timeLeft = 30;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra").Value;
            Texture2D tex3 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra2").Value;
            Texture2D tex4 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra3").Value;
            //Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(tex4, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f * Projectile.ai[2], Projectile.rotation, new Vector2(10, tex4.Height / 2), Projectile.scale * 1.1f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(0, 7.5f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(55, 52, 144) * 0.75f * Projectile.ai[2], Projectile.rotation + MathHelper.PiOver4 / 2f, new Vector2(0, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - new Vector2(0, 7.5f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(55, 52, 144) * 0.75f * Projectile.ai[2], Projectile.rotation - MathHelper.PiOver4 / 2f, new Vector2(0, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);



            Main.spriteBatch.Draw(tex3, Projectile.Center + new Vector2(0, 7.5f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(55, 52, 144) * 0.5f * Projectile.ai[2], Projectile.rotation + MathHelper.PiOver4 / 2f, new Vector2(15, tex3.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex3, Projectile.Center - new Vector2(0, 7.5f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(55, 52, 144) * 0.5f * Projectile.ai[2], Projectile.rotation - MathHelper.PiOver4 / 2f, new Vector2(15, tex3.Height / 2), Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(121, 152, 239) * 0.7f * Projectile.ai[2], Projectile.rotation, new Vector2(0, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, new Color(121, 152, 239) * 0.5f * Projectile.ai[2], Projectile.rotation, new Vector2(15, tex3.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f * Projectile.ai[2], Projectile.rotation, new Vector2(0, tex2.Height / 2), Projectile.scale * 1.1f, SpriteEffects.None, 0);

            float num = Projectile.localAI[0] / Projectile.ai[1];
            Player player = Main.player[Projectile.owner];
            float rot = (player.itemRotation - (player.direction == 1 ? MathHelper.PiOver4 : MathHelper.Pi - MathHelper.PiOver4));
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            for (int i = 0; i < 2; i++)
                Helper.DrawPrettyStarSparkle(Projectile.Center + new Vector2(35, 0).RotatedBy(rot) - Main.screenPosition, Color.White * Projectile.ai[2], Color.White * 0.1f * Projectile.ai[2], 0, Vector2.One * Projectile.ai[2], Vector2.One * num);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 30, Projectile.timeLeft);
            Projectile.ai[2] = MathHelper.Clamp((float)Math.Sin(progress * Math.PI), 0, 1);

            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            float num4 = (Projectile.rotation = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation);
            float num5 = 0.02f;
            float num6 = 1f;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + (player.itemRotation - (player.direction == 1 ? MathHelper.PiOver4 * 1.5f : MathHelper.Pi - MathHelper.PiOver4 * 1.5f)).ToRotationVector2() * 35;
            Projectile.scale = num6 + num * num5;
            Projectile.rotation = player.itemRotation - (player.direction == 1 ? MathHelper.PiOver4 * 1.5f : MathHelper.Pi - MathHelper.PiOver4 * 1.5f);

            float num11 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 vector7 = Projectile.Center + num11.ToRotationVector2() * 85f * Projectile.scale;
            Vector2 vector8 = (num11 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
            Color value2 = new Color(121, 152, 239);
            Lighting.AddLight(Projectile.Center, value2.ToVector3());
            //Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill();
            }
        }
    }
    public class EnchantedSwordP2 : ModProjectile
    {
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            float swingProgress = Ease(Utils.GetLerpValue(0f, 40, Projectile.timeLeft));
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 orig = texture.Size() / 2;
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (1f - fadeMult * i) * Projectile.ai[2], Projectile.oldRot[i] + (Projectile.ai[1] == -1 ? 0 : MathHelper.PiOver2 * 3), orig, Projectile.scale, Projectile.ai[1] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * Projectile.ai[2], Projectile.rotation + (Projectile.ai[1] == -1 ? 0 : MathHelper.PiOver2 * 3), orig, Projectile.scale, Projectile.ai[1] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EnchantedBeam);
            Projectile.aiStyle = -1;
            Projectile.Size = new Vector2(46, 46);
            Projectile.timeLeft = 40;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public virtual float Ease(float f)
        {
            return 1 - (float)Math.Pow(2, 10 * f - 10);
        }
        public virtual float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        Vector2 startP;
        public override void OnSpawn(IEntitySource source)
        {
            startP = Projectile.Center;
            //Projectile.ai[1] = Main.rand.Next(new int[] { 1, -1 });
        }
        public override void AI()
        {
            //if (Projectile.ai[1] == 0)
            //  Projectile.ai[1] = Main.rand.Next(new int[] { 1, -1 });
            if (startP == Vector2.Zero)
                startP = Projectile.Center;
            float direction = Projectile.ai[1];
            float swingProgress = Ease(Utils.GetLerpValue(0f, 40, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
            float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
            float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
            Vector2 position = startP +
                rotation.ToRotationVector2() * 30 * ScaleFunction(swingProgress);
            Projectile.Center = position;
            Projectile.rotation = (position - startP).ToRotation() + MathHelper.PiOver4;

            float progress = Utils.GetLerpValue(0, 50, Projectile.timeLeft);
            Projectile.ai[2] = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 2f, 0, 1);
        }
    }
}
