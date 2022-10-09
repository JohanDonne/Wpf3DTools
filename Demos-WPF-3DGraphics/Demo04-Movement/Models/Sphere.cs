using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Demo04_Movement.Models;

public class Sphere
{
    private double angle;
    public double Radius { get; init; }   

    public Point3D Position { get; private set; }    
    public double RotationsPerMinute { get; init; }
    public Sphere(double radius, Point3D position, double rotationsPerMinute)
    {
        Radius = radius;
        Position = position;
        RotationsPerMinute = rotationsPerMinute;
    }

    public void RotateAroundY(TimeSpan ellapsedTime)
    {
        angle = ellapsedTime.TotalMinutes * RotationsPerMinute * 360;
        // get horizontal component for position (as a vector)
        var vector = new Vector3D(Position.X, 0, Position.Z);
        // construct transformation matrix for the rotation
        var m = Matrix3D.Identity;
        var q = new Quaternion(new Vector3D(0, 1, 0), angle);
        m.Rotate(q);
        // and apply transformation to the horizontal component
        vector = m.Transform(vector);
        // and construct the new position by combining the heigth (Y-value) with the new horizontal component.
        Position = new Point3D(0, Position.Y, 0) + vector;
    }

}
