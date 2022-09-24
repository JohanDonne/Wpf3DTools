using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Implementation
{
    public class SphericalCameraController : ISphericalCameraController
    {
        // derived from SphericalCameraController by Rod Stephens
        // see: Wpf3D, isbn 9781983905964

        // The camera.
        public PerspectiveCamera Camera { get; } = new PerspectiveCamera();

        // Adjustment values.
        public double CameraDR => _cameraR / 50;
        public double CameraDTheta = Math.PI / 30;
        public double CameraDPhi = Math.PI / 15;

        // The current position.
        private double _cameraR = 30.0;
        private double _cameraTheta = Math.PI / 3.0;
        private double _cameraPhi = Math.PI / 3.0;

        // Get or set the spherical coordinates.
        // The point's coordinates are (r, theta, phi).
        public Point3D SphericalCoordinates
        {
            get => new(_cameraR, _cameraTheta, _cameraPhi);
            set
            {
                _cameraR = value.X;
                _cameraTheta = value.Y;
                _cameraPhi = value.Z;
            }
        }

        public Point3D CartesianCoordinates
        {
            get
            {
                SphericalToCartesian(_cameraR, _cameraTheta, _cameraPhi, out double x, out double y, out double z);
                return new Point3D(x, y, z);
            }
            set
            {
                CartesianToSpherical(value.X, value.Y, value.Z, out double r, out double theta, out double phi);
                _cameraR = r;
                _cameraTheta = theta;
                _cameraPhi = phi;
            }
        }

        public SphericalCameraController()
        {
            Camera.FieldOfView = 60;
            PositionCamera();
        }

        // Update the camera's position.
        public void IncreaseR(double amount)
        {
            _cameraR += amount;
            if (_cameraR < CameraDR) _cameraR = CameraDR;
        }
        public void IncreaseR()
        {
            IncreaseR(CameraDR);
        }
        public void DecreaseR(double amount)
        {
            IncreaseR(-amount);
        }
        public void DecreaseR()
        {
            IncreaseR(-CameraDR);
        }
        public void IncreaseTheta(double amount)
        {
            _cameraTheta += amount;
        }
        public void IncreaseTheta()
        {
            IncreaseTheta(CameraDTheta);
        }
        public void DecreaseTheta(double amount)
        {
            IncreaseTheta(-amount);
        }
        public void DecreaseTheta()
        {
            IncreaseTheta(-CameraDTheta);
        }
        public void IncreasePhi(double amount)
        {
            _cameraPhi += amount;
        }
        public void IncreasePhi()
        {
            IncreasePhi(CameraDPhi);
        }
        public void DecreasePhi(double amount)
        {
            IncreasePhi(-amount);
        }
        public void DecreasePhi()
        {
            IncreasePhi(-CameraDPhi);
        }

        #region Camera Control

        // Adjust the camera's position.
        public void ControlByKey(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    IncreasePhi();
                    break;
                case Key.Down:
                    DecreasePhi();
                    break;
                case Key.Left:
                    IncreaseTheta();
                    break;
                case Key.Right:
                    DecreaseTheta();
                    break;
                case Key.Add:
                case Key.OemPlus:
                    IncreaseR();
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    DecreaseR();
                    break;
            }

            // Update the camera's position.
            PositionCamera();
        }

        // Zoom in or out.
        public void Zoom(int delta)
        {
            DecreaseR(Math.Sign(delta) * CameraDR);
            PositionCamera();
        }

        public void ControlByMouse(Vector vector)
        {
            const double xscale = 0.03;
            const double yscale = 0.03;
            _cameraTheta -= vector.X * CameraDTheta * xscale;
            _cameraPhi -= vector.Y * CameraDPhi * yscale;
            PositionCamera();
        }

        public void PositionCamera(double radius, double theta, double phi)
        {
            SphericalCoordinates = new Point3D(radius, theta, phi);
            PositionCamera();
        }

        // Use the current values of CameraR, CameraTheta,
        // and CameraPhi to position the camera.
        private void PositionCamera()
        {
            // Calculate the camera's position in Cartesian coordinates.
            SphericalToCartesian(_cameraR, _cameraTheta, _cameraPhi,
                out double x, out double y, out double z);
            Camera.Position = new Point3D(x, y, z);

            // Look toward the origin.
            Camera.LookDirection = new Vector3D(-x, -y, -z);

            // Set the Up direction.
            Camera.UpDirection = new Vector3D(0, 1, 0);
        }

        // Convert from Cartesian to spherical coordinates.
        private static void CartesianToSpherical(double x, double y, double z,
            out double r, out double theta, out double phi)
        {
            r = Math.Sqrt((x * x) + (y * y) + (z * z));
            double h = Math.Sqrt((x * x) + (z * z));
            theta = Math.Atan2(x, z);
            phi = Math.Atan2(h, y);
        }

        // Convert from spherical to Cartesian coordinates.
        private static void SphericalToCartesian(double r, double theta, double phi,
            out double x, out double y, out double z)
        {
            y = r * Math.Cos(phi);
            double h = r * Math.Sin(phi);
            x = h * Math.Sin(theta);
            z = h * Math.Cos(theta);
        }

        #endregion Camera Control
    }
}
