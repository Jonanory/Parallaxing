using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Parallax
{
    [RequireComponent(typeof(Camera))]
    [Serializable]
    public class ParallaxCamera : MonoBehaviour
    {
        public static ParallaxCamera master;

        /// <summary>
        /// Whether the z value in an item's raw natural position is to be taken as a power of 10 units.
        /// e.g a z-value of 2 in the raw natural position means that the item is naturally 10^2 = 100 units away
        /// </summary>
        public bool logarithmicZDistances = false;

        /// <summary>
        /// If true, the player has to manually trigger the items to move using the function
        ///          ParallaxCamera.Notify("Camera Moved")
        /// If left unchecked, items are triggered on Update.
        /// </summary>
        public bool manualParallaxing = false;

        /// <summary>
        /// Whether to make the items reposition themselves in the z axis to keep consistent on what is supposed to be near and far away.
        /// </summary>
        public bool moveItemsInZDirection = false;

        private Vector3 lastPosition;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public List<ParallaxItem> items = new List<ParallaxItem>();

        /// <summary>
        /// The position that the player is relative to the center of the screen in units. If unchanged, will treat the player as if they are directly in front of the center of the screen.
        /// </summary>
        public Vector3 playerOffsetFromScreen = new Vector3(0, 0, 10);
        public Vector3 PlayerOffsetFromScreen
        {
            get { return playerOffsetFromScreen; }
            set
            {
                playerOffsetFromScreen = value;
                CalculateTanViewingAngle();
            }
        }

        private Vector2 tanViewingAngle = Vector2.zero;
        public Vector2 TanViewingAngle
        {
            get { return tanViewingAngle; }
        }

        void Awake()
        {
            if (master == null)
            {
                master = this;
            }
            else if (master != this)
            {
                Destroy(gameObject);
            }
            lastPosition = Position;
        }

        private void Update()
        {
            if (!manualParallaxing)
            {
                if (HasMoved())
                {
                    Notify("Camera Moved");
                }
            }
        }

        public static void Notify(string message)
        {
            foreach (ParallaxItem _item in master.items)
            {
                _item.Notify(message);
            }
        }

        void CalculateNaturalPosition()
        {
            Vector3 newPosition = transform.position;
            if (logarithmicZDistances)
            {
                newPosition.z = Mathf.Pow(10, newPosition.z);
            }
        }

        public bool HasMoved()
        {
            if (lastPosition != Position)
            {
                lastPosition = Position;
                return true;
            }
            return false;
        }

        private void CalculateTanViewingAngle()
        {
            tanViewingAngle.x = playerOffsetFromScreen.x / playerOffsetFromScreen.z;
            tanViewingAngle.y = playerOffsetFromScreen.y / playerOffsetFromScreen.z;
        }
    }
}
