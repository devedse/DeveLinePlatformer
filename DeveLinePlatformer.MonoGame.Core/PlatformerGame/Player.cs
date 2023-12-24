using DeveLinePlatformer.MonoGame.Core.HelperObjects;
using DeveLinePlatformer.MonoGame.Core.LineIntersection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    public class Player
    {
        private PointLine curStickyLine = null;
        private MapData mapData;
        private RectaleFloater pos;
        private Vector2 speed = new Vector2();
        private Boolean sticky = false;

        public Player(MapData mapData)
        {
            this.mapData = mapData;
            pos = new RectaleFloater(100, 100, 64, 128);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentDing.playerTexture, pos.ToRectangle(), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            PointLine oldStickyLine = null;

            if (InputDing.CurKey.IsKeyDown(Keys.R))
            {
                pos = new RectaleFloater(100, 100, 64, 128);
                sticky = false;
                curStickyLine = null;
                speed.Y = 0;
            }

            var previousPos = pos;

            if (InputDing.CurKey.IsKeyDown(Keys.Left))
            {
                speed.X = -10;
            }
            if (InputDing.CurKey.IsKeyDown(Keys.Right))
            {
                speed.X = 10;
            }

            if (InputDing.CurKey.IsKeyUp(Keys.Left) && InputDing.CurKey.IsKeyUp(Keys.Right))
            {
                speed.X = 0;
            }

            if (InputDing.KeyDownUp(Keys.Up))
            {
                speed.Y = -20.0f;
                sticky = false;
                oldStickyLine = curStickyLine;
                curStickyLine = null;
            }

            //Console.WriteLine(sticky);

            var speedRemainder = speed.X;
            var newPotentieelPootjesX = pos.PootjesX + speedRemainder;
        //pos.PootjesX += speed.X;

        restartcollision:

            if (!sticky)
            {
                speed.Y += 1.0f;

                pos.Bottom += speed.Y;

                if (oldStickyLine != null)
                {
                    if (newPotentieelPootjesX > oldStickyLine.LeftBall.Position.X &&
                        newPotentieelPootjesX < oldStickyLine.RightBall.Position.X)
                    {
                        Console.WriteLine("Potentieeltje");
                        if (pos.Bottom >= oldStickyLine.GetYHere(newPotentieelPootjesX))
                        {
                            Console.WriteLine("een echte");
                            //Apparently we slipped through the line we jumped from
                            sticky = true;
                            curStickyLine = oldStickyLine;
                        }
                    }

                }
                else if (speed.Y > 0)
                {
                    LineEquation l = new LineEquation(new Vector2(previousPos.PootjesX, previousPos.Bottom), new Vector2(newPotentieelPootjesX, pos.Bottom));
                    foreach (var line in mapData.lines)
                    {
                        var mapLine = new LineEquation(new Vector2(line.LeftBall.Position.X, line.LeftBall.Position.Y), new Vector2(line.RightBall.Position.X, line.RightBall.Position.Y));
                        Vector2 intersectionPoint;
                        Boolean intersect = l.ThisSegmentIntersectWithSegementOfLine(mapLine, out intersectionPoint);
                        //TODO find the closest line we are intersecting with
                        if (intersect)
                        {
                            speedRemainder = intersectionPoint.X - pos.PootjesX;
                            pos.PootjesX = intersectionPoint.X;
                            sticky = true;
                            curStickyLine = line;
                            break;
                        }
                    }
                }

                if (!sticky)
                {
                    //We did not intersect any lines
                    pos.PootjesX += speedRemainder;
                    speedRemainder = 0;
                }
            }

            if (sticky)
            {
                speed.Y = 0;

                //2
                if (speedRemainder > 0)
                {
                    //Moving right

                    while (curStickyLine != null && speedRemainder > 0)
                    {
                        var speedXAtThisLine = (float)Math.Cos(curStickyLine.Angle);
                        //0.2 = 2 * 0.1
                        var toMove = speedRemainder * speedXAtThisLine;

                        //15 + 0.2 - 15.15 = 0.05
                        var howFarBeyond = pos.PootjesX + toMove - curStickyLine.RightBall.Position.X;
                        if (howFarBeyond > 0)
                        {
                            //We used 75% of the speed to move to the next line
                            //We have 25% of the speed left
                            
                            //Percent
                            var howMuchPercentWeHaveRemaining = howFarBeyond / toMove;
                            speedRemainder = speedRemainder * howMuchPercentWeHaveRemaining;

                            var possibleLines = mapData.lines.Where(t => t.LeftBall.Position.Y == curStickyLine.RightBall.Position.Y && t.LeftBall.Position.X == curStickyLine.RightBall.Position.X)
                                .OrderBy(tt => tt.Angle);

                            var nextLine = possibleLines.FirstOrDefault(); //Highest line connecting to ball

                            if (nextLine != null)
                            {
                                pos.PootjesX = nextLine.LeftBall.Position.X;
                                curStickyLine = nextLine;
                            }
                            else
                            {
                                pos.Bottom = curStickyLine.LeftBall.Position.Y;
                                sticky = false;
                                curStickyLine = null;
                            }
                        }
                        else
                        {
                            speedRemainder = 0;
                            pos.PootjesX += toMove;
                        }
                    }
                }
                else if (speedRemainder < 0)
                {
                    //Moving left

                    while (curStickyLine != null && speedRemainder < 0)
                    {
                        var speedXAtThisLine = (float)Math.Cos(curStickyLine.Angle);
                        var toMove = speedRemainder * speedXAtThisLine;

                        var howFarBefore = pos.PootjesX + toMove - curStickyLine.LeftBall.Position.X;
                        if (howFarBefore < 0)
                        {
                            var howMuchPercentWeHaveRemaining = Math.Abs(howFarBefore / toMove);
                            speedRemainder = speedRemainder * howMuchPercentWeHaveRemaining;

                            var possibleLines = mapData.lines.Where(t => t.RightBall.Position.Y == curStickyLine.LeftBall.Position.Y && t.RightBall.Position.X == curStickyLine.LeftBall.Position.X)
                                                .OrderByDescending(tt => tt.Angle);

                            var nextLine = possibleLines.FirstOrDefault(); // Highest line connecting to ball on the left

                            if (nextLine != null)
                            {
                                pos.PootjesX = nextLine.RightBall.Position.X;
                                curStickyLine = nextLine;
                            }
                            else
                            {
                                pos.Bottom = curStickyLine.RightBall.Position.Y;
                                sticky = false;
                                curStickyLine = null;
                            }
                        }
                        else
                        {
                            speedRemainder = 0;
                            pos.PootjesX += toMove;
                        }
                    }
                }


                //while (curStickyLine != null && pos.PootjesX < curStickyLine.LeftBall.Position.X)
                //{
                //    var possibleLines = mapData.lines.Where(t => t.RightBall.Position.Y == curStickyLine.LeftBall.Position.Y && t.RightBall.Position.X == curStickyLine.LeftBall.Position.X)
                //        .OrderByDescending(tt => tt.Angle);

                //    var nextLine = possibleLines.FirstOrDefault(); //Highest line connecting to ball
                //    if (nextLine != null)
                //    {
                //        curStickyLine = nextLine;
                //    }
                //    else
                //    {
                //        pos.Bottom = curStickyLine.LeftBall.Position.Y;
                //        sticky = false;
                //        curStickyLine = null;
                //    }
                //}

                //while (curStickyLine != null && pos.PootjesX > curStickyLine.RightBall.Position.X)
                //{
                //    var possibleLines = mapData.lines.Where(t => t.LeftBall.Position.Y == curStickyLine.RightBall.Position.Y && t.LeftBall.Position.X == curStickyLine.RightBall.Position.X)
                //        .OrderBy(tt => tt.Angle);

                //    var nextLine = possibleLines.FirstOrDefault(); //Highest line connecting to ball

                //    if (nextLine != null)
                //    {
                //        curStickyLine = nextLine;
                //    }
                //    else
                //    {
                //        pos.Bottom = curStickyLine.RightBall.Position.Y;
                //        sticky = false;
                //        curStickyLine = null;
                //    }
                //}

                if (curStickyLine != null)
                {
                    pos.Bottom = curStickyLine.GetYHere(pos.PootjesX);
                }
                else
                {
                    //Falling off
                    previousPos = pos;
                    goto restartcollision;
                }
            }

            //var probable = mapData.lines.FirstOrDefault(t => t.LeftBall.Position.X < pos.PootjesX && t.RightBall.Position.X > pos.PootjesX);

            //if (probable != null)
            //{
            //    float valuetoset = probable.GetYHere(pos.PootjesX);
            //    pos.Bottom = valuetoset;
            //    //Console.WriteLine(valuetoset);
            //}
        }
    }
}
