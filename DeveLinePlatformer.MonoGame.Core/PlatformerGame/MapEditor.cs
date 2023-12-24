using DeveLinePlatformer.MonoGame.Core.ExtensionMethods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    internal class MapEditor
    {
        public List<PointBall> selectedBalls = new List<PointBall>();

        private PointBall draggingBall = null;
        private MapData mapData;
        private Vector2? startSelectionRectangle = null;

        public MapData MapData { get; private set; }

        public MapEditor(MapData mapData)
        {
            this.mapData = mapData;

            if (true)
            {
                float mmm = 20.0f;
                var preball = new PointBall(-100, 0);
                mapData.balls.Add(preball);
                float lastx = 0;
                for (float x = 0; x < 1400; x += mmm)
                {
                    var yone = (float)Math.Sin(x / 80.0) * 300 + 700;
                    var ytwo = (float)Math.Sin((x + 1) / 40.0) * 50 + 700;

                    var nextball = new PointBall(x + mmm, yone);
                    mapData.balls.Add(nextball);

                    AddLine(preball, nextball);
                    preball = nextball;
                    lastx = x;
                }

                //for (float x = lastx; x < 1600; x += mmm)
                //{
                //    var yone = (float)Math.Tan(x / 40.0) * 50 + 700;
                //    var ytwo = (float)Math.Tan((x + 1) / 40.0) * 50 + 700;

                //    var nextball = new PointBall(x + mmm, yone);
                //    mapData.balls.Add(nextball);

                //    AddLine(preball, nextball);
                //    preball = nextball;
                //    lastx = x;
                //}
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (startSelectionRectangle != null)
            {
                spriteBatch.DrawRectangle(5, Color.White, HelperMethods.RectangleFromVector(startSelectionRectangle.Value, InputDing.GetCurMousePos()));
            }

            var possibleSelectingBalls = new List<PointBall>();
            if (startSelectionRectangle != null)
            {
                var selectionRect = HelperMethods.RectangleFromVector(startSelectionRectangle.Value, InputDing.GetCurMousePos());
                possibleSelectingBalls = HelperMethods.GetCurrentlyCollidingBalls(mapData.balls, selectionRect);
            }

            foreach (var pointBall in mapData.balls)
            {
                pointBall.Draw(gameTime, spriteBatch, selectedBalls.Contains(pointBall), possibleSelectingBalls.Contains(pointBall));
            }

            foreach (var line in mapData.lines)
            {
                line.Draw(spriteBatch);
            }

            if (selectedBalls.Count == 1)
            {
                spriteBatch.DrawLine(3, Color.White, selectedBalls.First().Position, InputDing.GetCurMousePos());
            }

            spriteBatch.DrawString(ContentDing.spriteFont, "Lines: " + mapData.lines.Count + ", Balls: " + mapData.balls.Count + ", SelectedBalls: " + selectedBalls.Count + " PossibleSelectedBalls: " + possibleSelectingBalls.Count, new Vector2(10, 10), Color.White);
        }

        public void Update()
        {
            if (InputDing.CurMouse.LeftButton == ButtonState.Pressed && InputDing.PreMouse.LeftButton == ButtonState.Released)
            {
                startSelectionRectangle = InputDing.GetCurMousePos();
            }

            if (InputDing.CurMouse.LeftButton == ButtonState.Released && InputDing.PreMouse.LeftButton == ButtonState.Pressed)
            {
                var clickyBall = mapData.balls.FirstOrDefault(t => t.IsClickingOnIt(InputDing.CurMouse.X, InputDing.CurMouse.Y));

                if (HelperMethods.CirclesColliding(startSelectionRectangle.Value, InputDing.GetCurMousePos(), PointBall.Diameter))
                {
                    if (selectedBalls.Count == 1)
                    {
                        var selectedBall = selectedBalls.First();

                        if (clickyBall != null)
                        {
                            AddLine(selectedBall, clickyBall);
                            selectedBalls.Clear();
                            selectedBalls.Add(clickyBall);
                        }
                        else
                        {
                            //Clicking in nothing, create ball
                            var newBall = new PointBall(InputDing.CurMouse.X, InputDing.CurMouse.Y);
                            mapData.balls.Add(newBall);
                            AddLine(selectedBall, newBall);
                            selectedBalls.Clear();
                            selectedBalls.Add(newBall);
                        }
                    }
                    else
                    {
                        if (clickyBall != null)
                        {
                            selectedBalls.Clear();
                            selectedBalls.Add(clickyBall);
                        }
                        else
                        {
                            //Clicking in nothing, create ball
                            mapData.balls.Add(new PointBall(InputDing.CurMouse.X, InputDing.CurMouse.Y));
                        }
                    }
                }
                else
                {
                    var selectionRect = HelperMethods.RectangleFromVector(startSelectionRectangle.Value, InputDing.GetCurMousePos());
                    selectedBalls.Clear();
                    selectedBalls.AddRange(HelperMethods.GetCurrentlyCollidingBalls(mapData.balls, selectionRect));
                }
                startSelectionRectangle = null;
            }

            if (InputDing.CurMouse.RightButton == ButtonState.Pressed && InputDing.PreMouse.RightButton == ButtonState.Released)
            {
                var clickyBall = mapData.balls.FirstOrDefault(t => t.IsClickingOnIt(InputDing.CurMouse.X, InputDing.CurMouse.Y));
                if (clickyBall != null)
                {
                    draggingBall = clickyBall;
                }
                else
                {
                    selectedBalls.Clear();
                }
            }

            if (draggingBall != null)
            {
                var prePoss = draggingBall.Position;
                draggingBall.Position = InputDing.GetCurMousePos();
                var afterPos = draggingBall.Position;

                var diffPos = afterPos - prePoss;

                foreach (var selectedBall in selectedBalls)
                {
                    if (selectedBall != draggingBall)
                    {
                        selectedBall.Position += diffPos;
                    }
                }
            }

            if (InputDing.CurMouse.RightButton == ButtonState.Released && InputDing.PreMouse.RightButton == ButtonState.Pressed)
            {
                draggingBall = null;
            }

            if (InputDing.KeyDownUp(Keys.Delete))
            {
                foreach (var selectedBall in selectedBalls)
                {
                    mapData.lines.RemoveAll(t => t.LeftBall == selectedBall || t.RightBall == selectedBall);
                    mapData.balls.Remove(selectedBall);
                }
                selectedBalls.Clear();
            }
        }

        /// <summary>
        /// Shouldn't actually be needed due to awesome things, but still doing this
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void AddLine(PointBall first, PointBall second)
        {
            if (first == second)
                return;

            var leftBall = first.Position.X < second.Position.X ? first : second;
            var rightBall = leftBall == first ? second : first;

            if (!mapData.lines.Any(t => t.LeftBall == leftBall && t.RightBall == rightBall))
            {
                mapData.lines.Add(new PointLine(leftBall, rightBall));
            }
        }
    }
}
