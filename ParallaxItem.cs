using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Parallax
{
    public class ParallaxItem : MonoBehaviour
    {

        public bool useStartingTransformAsNaturalPosition = false;

        private ParallaxCamera pCamera;

        private Vector3 naturalPosition = Vector3.zero;
        public Vector3 NaturalPosition
        {
            get { return naturalPosition; }
        }

        [SerializeField]
        /// <summary>
        ///     The position of the item, before any logarithmic manipulation is taken into account
        /// </summary>
        private Vector3 naturalPositionRaw = Vector3.zero;
        public Vector3 NaturalPositionRaw
        {
            get { return naturalPositionRaw; }
            set
            {
                naturalPositionRaw = value;
                CalculateNaturalPosition();
            }
        }

        // Used in the calculation for parallaxing.
        float positionRatio = 1;

        void Awake()
        {
            if (useStartingTransformAsNaturalPosition)
            {
                naturalPositionRaw = transform.position;
            }
        }

        void Start()
        {
            pCamera = ParallaxCamera.master;
            CalculateNaturalPosition();
            pCamera.items.Add(this);
            Notify("New Camera");
        }

        void OnDestroy()
        {
            ParallaxCamera.master.items.Remove(this);
        }

        void CalculateNaturalPosition()
        {
            Vector3 newPosition = NaturalPositionRaw;
            if (ParallaxCamera.master.logarithmicZDistances)
            {
                newPosition.z = Mathf.Pow(10, NaturalPositionRaw.z);
            }
            naturalPosition = newPosition;
        }

        public void Notify(string _message)
        {
            switch (_message)
            {
                case "Camera Moved":
                    CalculatePosition(ParallaxCamera.master.Position);
                    break;
                case "New Camera":
                    SetRatio();
                    CalculatePosition(ParallaxCamera.master.Position);
                    break;
                case "Stop Parallaxing":
                    ResetPosition();
                    break;
                case "Start Parallaxing":
                    CalculatePosition(ParallaxCamera.master.Position);
                    break;
            }
        }

        private void CalculatePosition(Vector2 _cameraPosition)
        {
            //Accounting for players position relative to their screen
            Vector2 playerPosition = _cameraPosition - pCamera.playerOffsetFromScreen.z * pCamera.TanViewingAngle;

            Vector2 newItemPositionXY = positionRatio * (Vector2)NaturalPosition + (1 - positionRatio) * playerPosition;

            float newItemPositionZ = transform.position.z;
            if (pCamera.moveItemsInZDirection)
            {
                newItemPositionZ = NaturalPositionRaw.z + pCamera.playerOffsetFromScreen.z;
            }

            transform.position = new Vector3(
                newItemPositionXY.x,
                newItemPositionXY.y,
                newItemPositionZ
            );
        }

        public void ResetPosition()
        {
            transform.position = NaturalPositionRaw;
        }

        private void SetRatio()
        {
            positionRatio = pCamera.playerOffsetFromScreen.z / (pCamera.playerOffsetFromScreen.z + NaturalPosition.z - pCamera.Position.z);
        }
    }
}
