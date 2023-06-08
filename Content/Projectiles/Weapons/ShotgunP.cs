using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using Poganka.Content.Projectiles.Misc;

namespace Poganka.Content.Projectiles.Weapons
{
    public class ShotgunP : ModProjectile
    {
        public virtual float Ease(float f)
        {
            return 1 - (float)Math.Pow(2, 10 * f - 10);
        }
        public virtual float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        float holdOffset;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.Size = new(48, 18);
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 50;
            holdOffset = 20;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[1] = 1;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (Projectile.ai[2] > 0)
                Projectile.ai[2] -= 0.2f;
            //float progress = Ease(Utils.GetLerpValue(0f, 15, Projectile.timeLeft));
            if (Projectile.timeLeft == 46)
            {
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(Projectile.Center - new Vector2(-20, 4).RotatedBy(Projectile.rotation), 64, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(35)) * 2, Scale: 2).noGravity = true;
                    Dust.NewDustPerfect(Projectile.Center - new Vector2(-20, 4).RotatedBy(Projectile.rotation), 306, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(35)) * 2, Scale: 2).noGravity = true;
                }
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDustPerfect(Projectile.Center - new Vector2(-20, 4).RotatedBy(Projectile.rotation), DustID.Smoke, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(35)) * 2);
                }
                //player.ChooseAmmo(player.HeldItem).shoot
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center - new Vector2(0, 4).RotatedBy(Projectile.rotation), Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(35)) * 25, ModContent.ProjectileType<ShotgunPellet>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            if (Projectile.timeLeft > 35 && Projectile.timeLeft <= 45)
            {
                //holdOffset--;
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], MathHelper.ToRadians(29f * -Projectile.direction), 0.2f);
                /*if (Projectile.direction == -1)
                {
                    Projectile.ai[0] += MathHelper.ToRadians(2.25f);
                }
                else
                {
                    Projectile.ai[0] -= MathHelper.ToRadians(2.25f);
                }*/
            }
            else if (Projectile.timeLeft < 35 && Projectile.timeLeft >= 20)
            {
                /*if (Projectile.direction == -1)
                {
                    Projectile.ai[0] -= MathHelper.ToRadians(.9f * 4);
                }
                else
                {
                    Projectile.ai[0] += MathHelper.ToRadians(.9f * 4);
                }*/
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0, 0.03f);
            }
            else if (Projectile.timeLeft < 20 && Projectile.timeLeft >= 10)
            {
                Projectile.ai[1] -= 0.1f;
            }
            else if (Projectile.timeLeft < 10)
                Projectile.ai[1] += 0.1f;
            if (Projectile.timeLeft == 15)
            {
                Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), -1f), ModContent.Find<ModGore>("Poganka/ShotgunShell").Type);
            }

            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = (Projectile.velocity.ToRotation() + Projectile.ai[0]) * player.direction;
            pos += (Projectile.velocity.RotatedBy(Projectile.ai[0])) * holdOffset;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - MathHelper.PiOver2);// + Projectile.ai[0]);

            Projectile.rotation = (pos - player.Center).ToRotation() + Projectile.ai[0] * Projectile.spriteDirection;
            player.itemTime = 2;
            Projectile.Center = pos;
            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra").Value;
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, effects, 0);
            Main.EntitySpriteDraw(tex2, Projectile.Center + new Vector2(10 * (Projectile.ai[1]), -1).RotatedBy(Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, tex2.Size() / 2, Projectile.scale, effects, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)// && player.autoReuseAllWeapons)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    SoundEngine.PlaySound(SoundID.Item36, player.Center);
                    Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, dir, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0);
                    proj.Center = Projectile.Center;
                }
            }
        }
    }
}
