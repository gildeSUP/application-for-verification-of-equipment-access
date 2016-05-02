﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace frontEnd
{
    class validationObject
    {
        public Point3D currentPosition { get; private set; }
        public List<Point3D> trolley { get; private set; }
        public double width { get; private set; }
        public double length { get; private set; }
        public double height { get; private set; }
        public double angleXY { get; private set; }
        public double angleXZ { get; private set; }
        public double angleYZ { get; private set; }
        public double K { get; private set; }
        public double C { get;  private set; }
        public double mass { get; private set; }
        public List<Point3D> newPath { get; private set; }
        public double nextAngleXY { get; private set; }
        public double nextAngleXZ { get; private set; }
        public double nextAngleYZ { get; private set; }

        //initialize object parameters
        public validationObject(double width, double length, double height, double mass, Point3D startPos, Point3D RotateToPoint)
        {
            angleXY = 0;
            angleXZ = 0;
            angleYZ = 0;
            currentPosition = startPos;
            this.width = width;
            this.length = length;
            this.height = height;

            K = 0.1;
            this.mass = mass;
            C = 2 * Math.Sqrt(mass * K);

            newPath = new List<Point3D>();
            newPath.Add(startPos);

            trolley = new List<Point3D>();
            trolley.Add(new Point3D(startPos.X + (length / 2), startPos.Y + (width / 2), startPos.Z + (height / 2)));
            trolley.Add(new Point3D(startPos.X + (length / 2), startPos.Y + (width / 2), startPos.Z - (height / 2)));
            trolley.Add(new Point3D(startPos.X + (length / 2), startPos.Y - (width / 2), startPos.Z + (height / 2)));
            trolley.Add(new Point3D(startPos.X + (length / 2), startPos.Y - (width / 2), startPos.Z - (height / 2)));
            trolley.Add(new Point3D(startPos.X - (length / 2), startPos.Y - (width / 2), startPos.Z + (height / 2)));
            trolley.Add(new Point3D(startPos.X - (length / 2), startPos.Y - (width / 2), startPos.Z - (height / 2)));
            trolley.Add(new Point3D(startPos.X - (length / 2), startPos.Y + (width / 2), startPos.Z + (height / 2)));
            trolley.Add(new Point3D(startPos.X - (length / 2), startPos.Y + (width / 2), startPos.Z - (height / 2)));

            rotateTrolley(RotateToPoint);
        }
        public void updateObjectPosition(Vector3D distance)
        {
            currentPosition += distance;
            for (var i = 0; i < trolley.Count(); i++)
            {
                trolley[i] += distance;
            }
        }

        //TODO
        public void rotateTrolley(Point3D nextNode) 
        {

            nextAngleXY = Math.Atan2(nextNode.Y - currentPosition.Y, nextNode.X - currentPosition.X) * (180/Math.PI);
            nextAngleXZ = Math.Atan2(nextNode.Z - currentPosition.Z, nextNode.X - currentPosition.X) * (180 / Math.PI);
            nextAngleYZ = Math.Atan2(nextNode.Z - currentPosition.Z, nextNode.Y - currentPosition.Y) * (180 / Math.PI);           

            rotatePoints(nextAngleXY, nextAngleXZ, nextAngleYZ);
        }
        public void rotatePoints(double nextAngleXY, double nextAngleXZ, double nextAngleYZ)
        {
            if (nextAngleXY - angleXY != 0 || nextAngleXZ - angleXZ != 0 || nextAngleYZ - angleYZ != 0)
            {
                
                var group = new Transform3DGroup();
                var myRotateTransformZ = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), nextAngleXY-angleXY), currentPosition);
                group.Children.Add((myRotateTransformZ));
                /*var myRotateTransformY = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), nextAngleXZ - angleXZ), currentPosition);
                group.Children.Add((myRotateTransformY));
                var myRotateTransformX = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), nextAngleYZ - angleYZ), currentPosition);
                group.Children.Add((myRotateTransformX));*/
                for (var i = 0; i < trolley.Count(); i++)
                {
                    trolley[i] = group.Transform(trolley[i]);

                }
                angleXY = nextAngleXY;
                angleXZ = nextAngleXZ;
                angleYZ = nextAngleYZ;
            }
        }
        
            
    }
}
