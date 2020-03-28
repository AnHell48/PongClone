using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PongClone
{
    

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D _pallet_P1, _pallet_P2, _ball, _DEV_ball,_middle,select,_splashcreen,_over,_credits,_exit,_multi,_single,_start,_title,_AI_win,_P1win,_P2win,_by;
        Vector2 _pallet1_position, _pallet2_position,_ball_position, _DEV_ball_position,_middle_position,_score_position;
        Vector2 _multi_position,_credits_position ,_exit_position,_single_position;
        SpriteFont _DEV_info,_score_fontP1 ,_score_fontP2;
        int _scoreP1;
        int _scoreP2;
        bool _isSplahon = true;
        int key_pressed;
        float time = 3;
        KeyboardState keyB_state, keyB_prev;
        int counter = 0;
        //string test = "nada";
        bool _move_down, _move_right, Isover;
        public enum _GameState
        {
            SplashScreen,
            PressToContinue,
            Menu,
            Credits,
            SinglePlayer,
            MultiPlayer,
            Exit,
            GameOver
        }
        _GameState _currentGameState = new _GameState();
        Rectangle _ball_box;
        Vector2[] _select_position =  { new Vector2(220, 230), new Vector2(240, 270) , new Vector2(310, 310), new Vector2( 350, 350) };
        int _select_pos = 0;
        SoundEffect _SE_bounce, _SE_hit, _SE_score;
        SoundEffectInstance _SE_bounce_inst, _SE_hit_inst, _SE_score_inst;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // screen 800,480
            this.graphics.PreferredBackBufferWidth= 800;
            this.graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            _currentGameState = _GameState.SplashScreen;
            _move_down = true;
            _move_right = true;
            _middle_position = new Vector2(380,40);
            _pallet1_position = new Vector2(40, 200);
            _pallet2_position = new Vector2(760, 200);
            _ball_position = new Vector2(370, 200);
            _DEV_ball_position = new Vector2(0, 0);
            _score_position = new Vector2(30, 200);
            
            _single_position = new Vector2 ( 220, 230);
            _multi_position =new Vector2(240, 270);
            _credits_position= new Vector2(310, 310);
            _exit_position = new Vector2(350, 350);
            _scoreP1 = 0;
            _scoreP2 = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _ball = Content.Load<Texture2D>("ball");
            _pallet_P1 = Content.Load<Texture2D>("pallet");
            _pallet_P2 = Content.Load<Texture2D>("pallet");
            _middle = Content.Load<Texture2D>("mid");
            _score_fontP1 = Content.Load<SpriteFont>("score");
            _score_fontP2 = Content.Load<SpriteFont>("score");
            _splashcreen = Content.Load<Texture2D>("splashscreen");
            _credits = Content.Load<Texture2D>("pong_credits");
            _exit= Content.Load<Texture2D>("pong_exit"); 
            _multi= Content.Load<Texture2D>("pong_multi");
            _single = Content.Load<Texture2D>("pong_single");
            _start  = Content.Load<Texture2D>("pong_start");
            _title = Content.Load<Texture2D>("pong_main");
            _over = Content.Load<Texture2D>("pong_over");
            select = Content.Load<Texture2D>("pon_select");
            _by = Content.Load<Texture2D>("pong_by");
            _P1win = Content.Load<Texture2D>("pong_winP1");
            _P2win = Content.Load<Texture2D>("pong_winP2");
            _AI_win = Content.Load<Texture2D>("pong_AI");

            _SE_bounce = Content.Load<SoundEffect> ("pong_wall");
            _SE_bounce_inst = _SE_bounce.CreateInstance();
            _SE_hit = Content.Load<SoundEffect> ("pong_hit");
            _SE_hit_inst = _SE_hit.CreateInstance();
            _SE_score = Content.Load<SoundEffect> ("pong_score");
            _SE_score_inst = _SE_score.CreateInstance();

            // dev stuff
            _DEV_ball = Content.Load<Texture2D>("ball");
            _DEV_info = Content.Load<SpriteFont>("dev_info");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected bool CollitionP1()
        {
            Rectangle _pallet_box1 = new Rectangle((int)_pallet1_position.X,(int)_pallet1_position.Y,_pallet_P1.Width,_pallet_P1.Height);
            _ball_box = new Rectangle ((int)_ball_position.X,(int)_ball_position.Y,_ball.Width,_ball.Height);
            return  _ball_box.Intersects(_pallet_box1);
        }

        protected bool CollitionP2_AI()
        {
            Rectangle _pallet_box2 = new Rectangle((int)_pallet2_position.X, (int)_pallet2_position.Y, _pallet_P2.Width, _pallet_P2.Height);
           // _ball_box = new Rectangle((int)_ball_position.X, (int)_ball_position.Y, _ball.Width, _ball.Height);
            return _ball_box.Intersects(_pallet_box2);
        }

        public _GameState MenuSelection()
        {
            if (keyB_state.IsKeyDown(Keys.Up))
            {
                if (KeyPressed(Keys.Up))
                {
                    _select_pos -= 1;
                    if (_select_pos < 0)
                    {
                        _select_pos = 0;
                    }
                }
            }
            if (keyB_state.IsKeyDown(Keys.Down))
            {
                if (KeyPressed(Keys.Down))
                {
                    _select_pos += 1;
                    if (_select_pos > 3)
                    {
                        _select_pos = 3;
                    }
                }
            }
            if (keyB_state.IsKeyDown(Keys.Space) && _select_pos == 0)
            {
                    _currentGameState = _GameState.SinglePlayer;
            }
            else if (keyB_state.IsKeyDown(Keys.Space) && _select_pos == 1)
            {
                    _currentGameState = _GameState.MultiPlayer;
            }
            else if (keyB_state.IsKeyDown(Keys.Space) && _select_pos == 2)
            {
                    _currentGameState = _GameState.Credits;
            }
            else if (keyB_state.IsKeyDown(Keys.Space) && _select_pos == 3)
            {
                    _currentGameState = _GameState.Exit;
            }
            return _currentGameState;
        }

        protected void Score()
        {
            if (_ball_position.X < 1)
            {
                _scoreP1 ++;
                _SE_score.Play();
            }
            else if (_ball_position.X > 799)
            {
                _scoreP2++;
                _SE_score.Play();
            }
        }

        protected void ControlsArrow(Vector2 position, float speed)
        {
            //keyB_state = Keyboard.GetState();
            
            if (keyB_state.IsKeyDown(Keys.Up))
            {
                position.Y -= speed;
            }
            if (keyB_state.IsKeyDown(Keys.Down))
            {
                position.Y += speed;
            }
        }

        //protected void ControlsWASD(Vector2 position, float speed)
        //{
        //    keyB_state = Keyboard.GetState();

        //    if (keyB_state.IsKeyDown(Keys.W))
        //    {
        //        position.Y -= speed;
        //        test = "W";
        //    }
        //    if (keyB_state.IsKeyDown(Keys.S))
        //    {
        //        position.Y += speed;
        //        test = "S";
        //    }           
        //}

        protected bool IsOver()
        {
            if (_scoreP1 == 5 || _scoreP2 == 5)
            {
                Isover = true;
            }
            else
            {
                Isover = false;
            }
            return Isover;
        }

        protected void AI_Mov()
        {
            float difference = _ball_position.Y - _pallet2_position.Y;
             //AI pallet
            //if (_ball_position.X > 300)
            //{
            //    _pallet2_position.Y = _ball_position.Y ;
            //}
            if (difference >= 5 || difference <= 5)
            {
                if (difference > 0)
                {
                        _pallet2_position.Y += 6.3f;
                    
                }
                else 
                {
                    _pallet2_position.Y -= 8f;
                }
            }
        }

        protected void Movements()
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyB_state = Keyboard.GetState();

            #region DEV BALL
            if (keyB_state.IsKeyDown(Keys.L))
            {
                _DEV_ball_position.X += 4.0f;
            }
            if (keyB_state.IsKeyDown(Keys.J))
            {
                _DEV_ball_position.X -= 4.0f;
            }
            if (keyB_state.IsKeyDown(Keys.I))
            {
                _DEV_ball_position.Y -= 4.0f;
            }
            if (keyB_state.IsKeyDown(Keys.K))
            {
                _DEV_ball_position.Y += 4.0f;
            }
            #endregion

            // P1 pallet
            // ControlsArrow(_pallet1_position, 10.0f);

            if (keyB_state.IsKeyDown(Keys.Up))
            {
                _pallet1_position.Y -= 10.0f;
            }
            if (keyB_state.IsKeyDown(Keys.Down))
            {
                _pallet1_position.Y += 10.0f;
            }

            // P2 pallet
            #region P2Pallet
            // P2 join game
            if (keyB_state.IsKeyDown(Keys.Enter))
            {
                key_pressed = key_pressed + 1;
            }

            // bounds P2 _ AI
            if (_pallet2_position.Y < 0)
            {
                _pallet2_position.Y = 0;
            }
            else if (_pallet2_position.Y > 430)
            {
                _pallet2_position.Y = 430;
            }
            //if (key_pressed > 3)
            //{
            // ControlsWASD(_pallet2_position,10.0f);

            
            // }
            if (keyB_state.IsKeyDown(Keys.Space))
            {
                key_pressed = 0;
            }
            #endregion

            // bounds P1
            #region Bounds
            if (_pallet1_position.Y < 0)
            {
                _pallet1_position.Y = 0;
            }
            else if (_pallet1_position.Y > 430)
            {
                _pallet1_position.Y = 430;
            }

            #endregion

            // ball movement
            //timer to start
            #region Ball move
                if (_move_down)
                {
                    _ball_position.Y += 7.5f;
                    if (_ball_position.Y == 460)
                    {
                        _SE_bounce.Play();
                    }
                }
                else
                {
                    _ball_position.Y -= 7.5f;
                    if (_ball_position.Y == 0)
                    {
                        _SE_bounce.Play();
                    }
                }

                if (_move_right)
                {
                    _ball_position.X += 5.0f;
                }
                else
                {
                    _ball_position.X -= 5.0f;
                }

                // ball bounce up down
                if (_ball_position.Y > Window.ClientBounds.Height - _ball.Width)
                {
                    _move_down = !_move_down;
                }
                else if (_ball_position.Y < 0)
                {
                    _move_down = true;
                }

                // ball down left right

                if (_ball_position.X > 800)//Window.ClientBounds.Width - _ball.Height)
                {
                    //_move_right = !_move_right;
                    _ball_position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                }
                if (_ball_position.X < -3)
                {
                    //_move_right = true;
                    _ball_position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

                }
                #endregion
            
        }

        // this is to make a ''grid'' movement or just move once everytime a key is press
        protected bool KeyPressed(Keys key)
        {
            
            return keyB_state.IsKeyDown(key) && keyB_prev.IsKeyUp(key);
        }

        protected override void Update(GameTime gameTime)
        {
            keyB_prev = keyB_state; // 
            keyB_state = Keyboard.GetState();

            // selection between splash screen , menu, etc.
            switch (_currentGameState)
            {
                case _GameState.SplashScreen:
                   float time_pass = (float)gameTime.ElapsedGameTime.TotalSeconds;
                   time -= time_pass;// time + 1;
                   if (time <0)
                   {
                        _isSplahon = false;
                        _currentGameState = _GameState.PressToContinue;
                   } 
                   break;

                case _GameState.PressToContinue:
                   if (keyB_state.IsKeyDown(Keys.Enter) )
                    {
                        _currentGameState = _GameState.Menu;
                    }
                    break;
            
                case _GameState.Menu:
                    _scoreP1 = 0;
                    _scoreP2 = 0;
                    // select control
                    MenuSelection();
                    break;

                case _GameState.SinglePlayer:
                    //p1 controller
                    //ControlsArrow(_pallet1_position, 10.0f);
                   
                    AI_Mov();

                    if (!IsOver())
                    {
                        Movements();
                        Score();

                        if (CollitionP1() || CollitionP2_AI())
                        {
                            if (_move_right)
                            {
                                _move_right = !_move_right;
                                _SE_hit.Play();
                            }
                            else
                            {
                                _move_right = true;
                                _SE_hit.Play();
                            }
                        }
                    }
                    else
                    {
                        _currentGameState = _GameState.GameOver;
                    }
                    break;

                case _GameState.MultiPlayer:
                    if (keyB_state.IsKeyDown(Keys.W))
                    {
                        _pallet2_position.Y -= 10.0f;
                    }
                    if (keyB_state.IsKeyDown(Keys.S))
                    {
                        _pallet2_position.Y += 10.0f;
                    }
                    Movements();
                    if (!IsOver())
                    {
                        Movements();
                        Score();

                        if (CollitionP1() || CollitionP2_AI())
                        {
                            if (_move_right)
                            {
                                _move_right = !_move_right;
                                _SE_hit.Play();
                            }
                            else
                            {
                                _move_right = true;
                                _SE_hit.Play();
                            }
                        }
                    }
                    else
                    {
                        _currentGameState = _GameState.GameOver;
                    }
                    break;

                case _GameState.Credits:
                    if (keyB_state.IsKeyDown(Keys.Enter))
                    {
                        if (KeyPressed(Keys.Enter))
                        {
                            _currentGameState = _GameState.Menu;
                        }
                    }
                    break;
                case _GameState.GameOver:
                    if (keyB_state.IsKeyDown(Keys.Enter))
                    {
                        _currentGameState = _GameState.Menu;
                    }
                    break;
                case _GameState.Exit:
                    if (keyB_state.IsKeyDown(Keys.Enter) || keyB_state.IsKeyDown(Keys.Space))
                    {
                        Exit();
                    }
                    break;  
                default:
                    Exit();
                    break;
            }


            //_ball_position.X += speedX;
            //if (_ball_position.X > Window.ClientBounds.Width - _ball.Width || _ball_position.X < 0)
            //{ speedX *= -1f; }

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (_isSplahon)
            {
                spriteBatch.Draw(_splashcreen, new Vector2(250, 150), Color.White);

            }
             switch (_currentGameState) 
            {
                case _GameState.PressToContinue:
                    spriteBatch.Draw(_title, new Vector2(110, 15), Color.White);
                    spriteBatch.Draw(_start, new Vector2(300, 300), Color.White);
                    break;
            
               case _GameState.Menu:
                     
                    spriteBatch.Draw(_title, new Vector2(110, 15), Color.White);
                    spriteBatch.Draw(select, _select_position[_select_pos], Color.White);
                    spriteBatch.Draw(_single, _single_position, Color.White);
                    spriteBatch.Draw(_multi, _multi_position, Color.White);
                    spriteBatch.Draw(_credits, _credits_position, Color.White);
                    spriteBatch.Draw(_exit,_exit_position, Color.White);
                    break;

               case _GameState.SinglePlayer:
                 case _GameState.MultiPlayer:
                    if (!IsOver())
                    {
                        // Update(GameTime gameTime);
                        spriteBatch.Draw(_middle, _middle_position, Color.White);
                        spriteBatch.Draw(_pallet_P1, _pallet1_position, Color.White);
                        spriteBatch.Draw(_pallet_P2, _pallet2_position, Color.White);
                        spriteBatch.Draw(_ball, _ball_position, Color.White);

                        // dev stuff
                        //spriteBatch.Draw(_DEV_ball, _DEV_ball_position, Color.Fuchsia);
                        // spriteBatch.DrawString(_DEV_info, "position:" + time, new Vector2(20, 420), Color.Red);

                        //fonts
                        spriteBatch.DrawString(_score_fontP1, "" + _scoreP1, new Vector2(260, 60), Color.White);
                        spriteBatch.DrawString(_score_fontP2, "" + _scoreP2, new Vector2(470, 60), Color.White);
                    }
                    else {
                        _currentGameState = _GameState.GameOver;
                    }
                         break;
                 case _GameState.Credits:
                         spriteBatch.Draw(_by,new Vector2(200,150), Color.White);
                         break;

                 case _GameState.GameOver:
                    spriteBatch.Draw(_over, new Vector2(200, 100), Color.White);
                    //this.Exit();
                    break;
                
            }
             spriteBatch.DrawString(_DEV_info, "" , new Vector2(20, 420), Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
