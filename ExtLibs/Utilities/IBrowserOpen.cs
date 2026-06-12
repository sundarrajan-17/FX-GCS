using System;

namespace XagSurveillanceGCS.Utilities
{
    public interface IBrowserOpen
    {
        bool OpenURL(Uri uri);
    }
}