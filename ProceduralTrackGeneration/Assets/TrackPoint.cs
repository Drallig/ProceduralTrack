using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPoint : MonoBehaviour
{
    enum Type
    {
        Active,
        Runoff,
        Inactive
    };

    //this point in active track
    public Vector3 Position;
    public TrackPoint nextPoint;
    public TrackPoint lastPoint;

    //compass points
    public TrackPoint n;
    public TrackPoint ne;
    public TrackPoint e;
    public TrackPoint se;
    public TrackPoint s;
    public TrackPoint sw;
    public TrackPoint w;
    public TrackPoint nw;


}
