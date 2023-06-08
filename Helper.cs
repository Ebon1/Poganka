﻿using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using System.IO;
using System.Text;
using static Terraria.ModLoader.ModContent;
using Poganka;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;
using Terraria.Graphics.Shaders;
using System.ComponentModel;
using static tModPorter.ProgressUpdate;
using System.Linq;
using System.Runtime;

namespace Poganka
{
    public struct Text
    {
        public string text;
        public Rectangle rect;
        public string wrappedString;
        public Text(Rectangle rect, DynamicSpriteFont font, string text)
        {
            this.text = text;
            this.rect = rect;
            this.wrappedString = Helper.WrapText(font, this.text, this.rect.Width);
        }
    }

    public static class Helper
    {

        public static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {

            Texture2D value = TextureAssets.Extra[98].Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawpos, null, color, (float)Math.PI / 2f + rotation, origin, vector, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color, 0f + rotation, origin, vector2, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, 0f + rotation, origin, vector2 * 0.6f, dir);
        }

        public static void DrawPrettyStarSparkle(Vector2 drawpos, Color drawColor, Color shineColor, float rotation, Vector2 scale, Vector2 fatness)
        {
            SpriteEffects dir = SpriteEffects.None;
            Texture2D value = TextureAssets.Extra[98].Value;
            Color color = shineColor * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            Main.EntitySpriteDraw(value, drawpos, null, color, (float)Math.PI / 2f + rotation, origin, scale, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color, 0f + rotation, origin, scale, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, (float)Math.PI / 2f + rotation, origin, scale * 0.6f, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, 0f + rotation, origin, scale * 0.6f, dir);
        }

        public static void DrawPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, bool drawBacksides = true)
        {
            if (vertices.Length < 6) return;
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = Poganka.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
            effect.CurrentTechnique.Passes["Default"].Apply();
            if (drawBacksides)
            {
                short[] indices = new short[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    indices[i * 2] = (short)i;
                    indices[i * 2 + 1] = (short)(i + 1);
                    indices[i * 2 + 2] = (short)(i + 2);

                    indices[i * 2 + 5] = (short)i;
                    indices[i * 2 + 4] = (short)(i + 1);
                    indices[i * 2 + 3] = (short)(i + 2);
                }

                device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                    GetPrimitiveCount(vertices.Length, type) * 2);
            }
            else
            {
                device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
            }
        }
        public static void DrawTexturedPrimitives(VertexPositionColorTexture[] vertices, PrimitiveType type, Texture2D texture, bool drawBacksides = true)
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;
            Effect effect = Poganka.TrailShader;
            effect.Parameters["WorldViewProjection"].SetValue(GetMatrix());
            effect.Parameters["tex"].SetValue(texture);
            effect.CurrentTechnique.Passes["Texture"].Apply();
            if (drawBacksides)
            {
                short[] indices = new short[vertices.Length * 2];
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    indices[i * 2] = (short)i;
                    indices[i * 2 + 1] = (short)(i + 1);
                    indices[i * 2 + 2] = (short)(i + 2);

                    indices[i * 2 + 5] = (short)i;
                    indices[i * 2 + 4] = (short)(i + 1);
                    indices[i * 2 + 3] = (short)(i + 2);
                }

                device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indices, 0,
                    GetPrimitiveCount(vertices.Length, type) * 2);
            }
            else
            {
                device.DrawUserPrimitives(type, vertices, 0, GetPrimitiveCount(vertices.Length, type));
            }
        }
        public static float ClosestTo(this IEnumerable<float> collection, float target)
        {
            // NB Method will return int.MaxValue for a sequence containing no elements.
            // Apply any defensive coding here as necessary.
            var closest = float.MaxValue;
            var minDifference = float.MaxValue;
            foreach (var element in collection)
            {
                var difference = Math.Abs(element - target);
                if (minDifference > difference)
                {
                    minDifference = difference;
                    closest = element;
                }
            }

            return closest;
        }
        public static int IndexOfClosestTo(this IEnumerable<float> collection, float target)
        {
            // NB Method will return int.MaxValue for a sequence containing no elements.
            // Apply any defensive coding here as necessary.
            int closest = 0;
            var minDifference = float.MaxValue;
            foreach (float element in collection)
            {
                var difference = Math.Abs(element - target);
                if (minDifference > difference)
                {
                    minDifference = difference;
                    closest = Array.IndexOf(collection.ToArray(), element);
                }
            }

            return closest;
        }
        public static float Closer(float a, float b, float compareValue)
        {

            float calcA = Math.Abs(a - compareValue);
            float calcB = Math.Abs(b - compareValue);

            if (calcA == calcB)
            {
                return 0;
            }

            if (calcA < calcB)
            {
                return a;
            }

            return b;
        }
        /// <summary>
        /// Extremely laggy grounded check, only use this for stuff like death animations where you absolutely dont want the npc to not be able to detect the ground
        /// </summary>
        /// 
        public static bool Grounded(this NPC NPC, float offset = .5f, float offsetX = 1f)
        {
            if (NPC.collideY)
                return true;
            if ((!Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 2), 1, 1, new Vector2(NPC.Center.X, NPC.Center.Y + (NPC.height * offset) / 2), 1, 1) || Collision.FindCollisionDirection(out int dir, NPC.Center, 1, NPC.height / 2)))
                return true;
            for (int i = 0; i < NPC.width * offsetX; i += (int)(1 / (offsetX == 0 ? 1 : offsetX))) //full sprite check
            {

                bool a = TRay.CastLength(NPC.BottomLeft + Vector2.UnitX * i, Vector2.UnitY, 1000) < NPC.height * offset;
                if (!a)
                    continue;
                return a;
            }
            return false; //give up
        }
        public static string RgbToHex(Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;
            return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
        }
        public static VertexPositionColorTexture AsVertex(Vector2 position, Color color, Vector2 texCoord)
        {
            return new VertexPositionColorTexture(new Vector3(position, 50), color, texCoord);
        }
        public static VertexPositionColorTexture AsVertex(Vector3 position, Color color, Vector2 texCoord)
        {
            return new VertexPositionColorTexture(position, color, texCoord);
        }
        private static int width;
        private static int height;
        private static Vector2 zoom;
        private static Matrix view;
        private static Matrix projection;
        private static bool CheckGraphicsChanged()
        {
            var device = Main.graphics.GraphicsDevice;
            bool changed = device.Viewport.Width != width
                           || device.Viewport.Height != height
                           || Main.GameViewMatrix.Zoom != zoom;

            if (!changed) return false;

            width = device.Viewport.Width;
            height = device.Viewport.Height;
            zoom = Main.GameViewMatrix.Zoom;

            return true;
        }

        /// <summary>
        /// used for uuhhhhh evenly distrubuted velocity or sum shit i forgor
        /// </summary>
        public static float CircleDividedEqually(float i, float max)
        {
            return 2f * (float)Math.PI / max * i;
        }
        public static Matrix GetMatrix()
        {
            if (CheckGraphicsChanged())
            {
                var device = Main.graphics.GraphicsDevice;
                int width = device.Viewport.Width;
                int height = device.Viewport.Height;
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                view =
                    Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up)
                    * Matrix.CreateTranslation(width / 2, height / -2, 0)
                    * Matrix.CreateRotationZ(MathHelper.Pi)
                    * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            }

            return view * projection;
        }

        private static int GetPrimitiveCount(int vertexCount, PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.LineList:
                    return vertexCount / 2;
                case PrimitiveType.LineStrip:
                    return vertexCount - 1;
                case PrimitiveType.TriangleList:
                    return vertexCount / 3;
                case PrimitiveType.TriangleStrip:
                    return vertexCount - 2;
                default: return 0;
            }
        }

        public static void Reload(this SpriteBatch spriteBatch, SamplerState _samplerState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = _samplerState;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }

        public static Vector2 GetRotation(List<Vector2> oldPos, int index)
        {
            if (oldPos.Count == 1)
                return oldPos[0];

            if (index == 0)
            {
                return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);
            }

            return (index == oldPos.Count - 1
                ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
                : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
        }

        public static string WrapText(DynamicSpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
        public static string Empty = "Poganka/Assets/Tex/Extras/Empty";
        public static string Placeholder = "Poganka/Assets/Tex/Extras/Placeholder";
        public static class TRay
        {
            public static Vector2 Cast(Vector2 start, Vector2 direction, float length, bool platformCheck = false)
            {
                direction = direction.SafeNormalize(Vector2.UnitY);
                Vector2 output = start;

                for (int i = 0; i < length; i++)
                {
                    if (Collision.CanHitLine(output, 0, 0, output + direction, 0, 0) && (platformCheck ? !Collision.SolidTiles(output, 1, 1, platformCheck) : true))
                    {
                        output += direction;
                    }
                    else
                    {
                        break;
                    }
                }

                return output;
            }
            public static float CastLength(Vector2 start, Vector2 direction, float length, bool platformCheck = false)
            {
                Vector2 end = Cast(start, direction, length, platformCheck);
                return (end - start).Length();
            }
        }
        public static Texture2D GetExtraTexture(string tex, bool altMethod = false)
        {
            if (altMethod)
                return GetTextureAlt("Assets/Tex/Extras/" + tex);
            return GetTexture("Assets/Tex/Extras/" + tex);
        }
        public static Texture2D GetTexture(string path)
        {
            return ModContent.Request<Texture2D>("Poganka/" + path).Value;
        }
        public static Texture2D GetTextureAlt(string path)
        {
            return Poganka.Instance.Assets.Request<Texture2D>(path).Value;
        }
        public static Vector4 ColorToVector4(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static Vector4 ColorToVector4(Vector4 color)
        {
            return new Vector4(color.X / 255f, color.Y / 255f, color.Z / 255f, color.W / 255f);
        }
        public static Vector4 ColorToVector4(Vector3 color)
        {
            return new Vector4(color.X / 255f, color.Y / 255f, color.Z / 255f, 1);
        }
        //public static Player[] activePlayers = new Player[Main.maxPlayers];
        //public static Player GetRandomPlayer()
        //{
        //   return Main.player[Main.rand.Next(activePlayers.Length)];
        //}
        public static SpriteSortMode previousSortMode;
        public static BlendState previousBlendState;
        public static SamplerState previousSamplerState;
        public static DepthStencilState previousDepthStencilState;
        public static RasterizerState previousRasterizerState;
        public static Effect previousEffect;
        public static Matrix previousMatrix;

        public static void SaveCurrent(this SpriteBatch spriteBatch)
        {
            previousSortMode = SpriteSortMode.Deferred;
            previousBlendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousSamplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousDepthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousRasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousEffect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            previousMatrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        }

        public static void ApplySaved(this SpriteBatch spriteBatch)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = previousSortMode;
            BlendState blendState = previousBlendState;
            SamplerState samplerState = previousSamplerState;
            DepthStencilState depthStencilState = previousDepthStencilState;
            RasterizerState rasterizerState = previousRasterizerState;
            Effect effect = previousEffect;
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, previousMatrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, Effect effect = null)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
        {
            projectile.ai[1]++;
            float wave = (float)Math.Sin(projectile.ai[1] * frequencyMultiplier);
            Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
            vector.Normalize();
            wave *= projectile.ai[0];
            wave *= amplitude;
            Vector2 offset = vector * wave;
            projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
            projectile.Center = projectile.Center + offset;
        }
        public static Vector2 FromAToB(Vector2 a, Vector2 b, bool normalize = true, bool reverse = false)
        {
            Vector2 baseVel = b - a;
            if (normalize)
                baseVel.Normalize();
            if (reverse)
            {
                Vector2 baseVelReverse = a - b;
                if (normalize)
                    baseVelReverse.Normalize();
                return baseVelReverse;
            }
            return baseVel;
        }

        public static void Log(this Projectile obj)
        {
            Main.NewText("Friendly?" + obj.friendly);
            Main.NewText("Hostile?" + obj.hostile);
            Main.NewText("Object:" + obj.Name);
            Main.NewText("Timeleft:" + obj.timeLeft);
            Main.NewText("Damage:" + obj.damage);
            Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + "]");
            Main.NewText("Direction:" + obj.direction);
            Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + "]");
            Main.NewText("Velocity:" + obj.velocity);
            Main.NewText("Owner:" + obj.owner);
        }
        public static void Log(this NPC obj)
        {
            Main.NewText("Friendly?" + obj.friendly);
            Main.NewText("Object:" + obj.TypeName);
            Main.NewText("Timeleft:" + obj.timeLeft);
            Main.NewText("Damage:" + obj.damage);
            Main.NewText("AI: [" + obj.ai[0] + ", " + obj.ai[1] + ", " + obj.ai[2] + ", " + obj.ai[3] + "]");
            Main.NewText("Direction:" + obj.direction);
            Main.NewText("LocalAI: [" + obj.localAI[0] + ", " + obj.localAI[1] + ", " + obj.localAI[2] + ", " + obj.localAI[3] + "]");
            Main.NewText("Velocity:" + obj.velocity);
        }
        public static void CollisionTPNoDust(Vector2 targetPosition, Player player)
        {
            int num = 150;
            Vector2 vector = player.position;
            Vector2 vector2 = player.velocity;
            for (int i = 0; i < num; i++)
            {
                vector2 = (vector + player.Size / 2f).DirectionTo(targetPosition).SafeNormalize(Vector2.Zero) * 12f;
                Vector2 vector3 = Collision.TileCollision(vector, vector2, player.width, player.height, fallThrough: true, fall2: true, (int)player.gravDir);
                vector += vector3;
            }
            _ = vector - player.position;
            TPNoDust(vector, player);
            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, vector.X, vector.Y, 0);
        }
        public static void TPNoDust(Vector2 newPos, Player player)
        {
            try
            {
                player._funkytownAchievementCheckCooldown = 100;
                player.environmentBuffImmunityTimer = 4;
                player.RemoveAllGrapplingHooks();
                player.StopVanityActions();
                float num = MathHelper.Clamp(1f - player.teleportTime * 0.99f, 0.01f, 1f);
                Vector2 otherPosition = player.position;
                float num2 = Vector2.Distance(player.position, newPos);
                PressurePlateHelper.UpdatePlayerPosition(player);
                player.position = newPos;
                player.fallStart = (int)(player.position.Y / 16f);
                if (player.whoAmI == Main.myPlayer)
                {
                    bool flag = false;
                    if (num2 < new Vector2(Main.screenWidth, Main.screenHeight).Length() / 2f + 100f)
                    {
                        int time = 0;
                        Main.SetCameraLerp(0.1f, time);
                        flag = true;
                    }
                    else
                    {
                        NPC.ResetNetOffsets();
                        Main.BlackFadeIn = 255;
                        Lighting.Clear();
                        Main.screenLastPosition = Main.screenPosition;
                        Main.screenPosition.X = player.position.X + (float)(player.width / 2) - (float)(Main.screenWidth / 2);
                        Main.screenPosition.Y = player.position.Y + (float)(player.height / 2) - (float)(Main.screenHeight / 2);
                        Main.instantBGTransitionCounter = 10;
                        player.ForceUpdateBiomes();
                    }
                }
                PressurePlateHelper.UpdatePlayerPosition(player);
                player.ResetAdvancedShadows();
                for (int i = 0; i < 3; i++)
                {
                    player.UpdateSocialShadow();
                }
                player.oldPosition = player.position + player.BlehOldPositionFixer;
            }
            catch
            {
            }
        }
        public static void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
        {
            Dust.QuickDust(start, color).scale = 1f;
            Dust.QuickDust(end, color).scale = 1f;
            float num = 1f / splits;
            for (float amount = 0.0f; (double)amount < 1.0; amount += num)
                Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
        }
        public static void QuickDustLine(this Dust dust, Vector2 start, Vector2 end, float splits, Color color1, Color color2)
        {
            Dust.QuickDust(start, color1).scale = 1f;
            Dust.QuickDust(end, color2).scale = 1f;
            float num = 1f / splits;
            for (float amount = 0.0f; (double)amount < 1.0; amount += num)
            {
                Color color = Color.Lerp(color1, color2, amount);
                Dust.QuickDustSmall(Vector2.Lerp(start, end, amount), color).scale = 1f;
            }
        }
    }
    public class MiscDrawingMethods
    {
        public static readonly BlendState Subtractive = new BlendState
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };
        public readonly static BlendState AlphaSubtractive = new BlendState
        {
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };
        public static void DrawWithDye(SpriteBatch spriteBatch, DrawData data, int dye, Entity entity, bool Additive = false)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, Additive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            //DrawData a = new(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, 1, SpriteEffects.None, 0);
            GameShaders.Armor.GetShaderFromItemId(dye).Apply(entity, data);
            data.Draw(Main.spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
        }
    }
}