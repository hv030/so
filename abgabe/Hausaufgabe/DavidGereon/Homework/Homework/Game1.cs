using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Homework
{
    internal sealed class Game1 : Game
    {
        private SpriteBatch mSpriteBatch;

        private Texture2D mLogo;
        private Texture2D mBackground;
        private SoundEffect mHit;
        private SoundEffect mMiss;
        private readonly Vector2 mWindowSize;
        private Vector2 mLogoPosition;
        private Vector2 mLogoOffset;
        private float mLogoScale;
        private float mTurnCount;


        public Game1()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            mWindowSize = new Vector2(960, 768);
            graphics.PreferredBackBufferWidth = (int)mWindowSize.X;
            graphics.PreferredBackBufferHeight = (int)mWindowSize.Y;
            graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            mLogoOffset = mWindowSize / 2;
            mLogoScale = 0.15f;
            mTurnCount = 0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mLogo = Content.Load<Texture2D>("logo");
            mBackground = Content.Load<Texture2D>("Background");
            mHit = Content.Load<SoundEffect>("Logo_hit");
            mMiss = Content.Load<SoundEffect>("Logo_miss");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }
            mTurnCount += 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            mLogoPosition.X = (float)Math.Cos(mTurnCount) * 200 + mLogoOffset.X;
            mLogoPosition.Y = (float)Math.Sin(mTurnCount) * 200 + mLogoOffset.Y;
            HitLogo();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mBackground, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
            mSpriteBatch.Draw(mLogo, mLogoPosition - mLogo.Bounds.Center.ToVector2() * mLogoScale, null, Color.White, 0f, Vector2.Zero, mLogoScale, SpriteEffects.None, 0f);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        private void HitLogo()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed || !InWindow()) { }
            else
            {
                if (Distance(Mouse.GetState().Position.ToVector2(), mLogoPosition) <= mLogo.Width * mLogoScale / 2)
                {
                    mHit.CreateInstance().Play();
                }
                else
                {
                    mMiss.CreateInstance().Play();
                }
            }
        }

        private float Distance(Vector2 a, Vector2 b) { return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)); }
        private bool InWindow() { return !(Mouse.GetState().X < 0 || Mouse.GetState().X > mWindowSize.X || Mouse.GetState().Y < 0 || Mouse.GetState().Y > mWindowSize.Y); }
    }
}
