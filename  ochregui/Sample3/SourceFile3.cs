using System;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Sample3
{
    /// <summary>
    /// The MapView will be responsible for drawing the player, and provides an area for the player
    /// to roam in.
    /// </summary>
    class MapView : Panel
    {

        /// Again we force that a Player object be passed to our constructor, and we save it in
        /// a field for later use.
        public MapView(PanelTemplate template,Player player)
            : base(template)
        {
            this.player = player;
        }

        Player player;


        /// Here is our drawing code, and it illustrates a couple of things.  First is the ScaleValue
        /// method of the Color class, which is basically a straight wrapper around the libtcod version.
        /// Also note that Colors can be constructed by specifying seperate R,G,B components.
        /// Later we are going to take advantage of the Intensity to add a flickering animation to the
        /// player.
        protected override void Redraw()
        {
            base.Redraw();


            Color fg = player.BaseColor.ScaleValue(player.Intensity);
            Color bg = new Color(0, 0, 0);

            Canvas.PrintChar(player.CurrentPosition, '@',new Pigment(fg,bg));
        }



        /// This method will be called by our MyManager object when we want to move the player.
        internal void MovePlayer(int dx, int dy)
        {
            /// Create the new position based on the current postion adn the delta values.
            Point newPos = player.CurrentPosition.Shift(dx, dy);

            /// Widget.LocalRect returns the region of the widget in local space coordinates (local to
            /// that Widget).  This is a convenience method, and the upper left corner of the rect
            /// will always be at 0,0.
            /// 
            /// Rect.Contains(Point) returns true if the specified point is somewhere inside the Rect.
            /// In this case, we are checking if the new position caluclated above is inside the 
            /// MapView region.  If so, we set the player's position to the new position.
            if (LocalRect.Contains(newPos))
            {
                player.CurrentPosition = newPos;
            }
        }

        /// <summary>
        /// Another method called from MyManager.  This method randomly sets the player position
        /// within the map view.
        /// </summary>
        internal void TeleportPlayer()
        {
            int newX = Rand.Int32(0, Size.Width - 1);
            int newY = Rand.Int32(0, Size.Height - 1);

            player.CurrentPosition = new Point(newX, newY);
        }


        /// The next two methods should be self explanatory.  We will call these from MyManager as well.

        internal void SetPlayerColor(Color newColor)
        {
            player.BaseColor = newColor;
        }

        internal void SetPlayerIntensity(float intensity)
        {
            player.Intensity = intensity;
        }



        /// Finally, we show a tooltip when the mouse is over the player.  DetermineTooltipText() is called by
        /// the base class Control when the mouse enters a hover state.  The base implementation of
        /// DetermineTooltipText simply returns the TooltipText property.  Here we override it to handle
        /// more complex behavior.
        /// 
        /// Note that we have to translate the mouse position to local coordinates.
        protected override string DetermineTooltipText()
        {
            if (this.ScreenToLocal(CurrentMousePos) == player.CurrentPosition)
                return "It's you!";
            else
                return null;
        }
    }
}
