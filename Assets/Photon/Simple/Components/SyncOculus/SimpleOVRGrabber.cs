// ---------------------------------------------------------------------------------------------
// <copyright>PhotonNetwork Framework for Unity - Copyright (C) 2020 Exit Games GmbH</copyright>
// <author>developer@exitgames.com</author>
// ---------------------------------------------------------------------------------------------

#if !OCULUS
using UnityEngine;
namespace Photon.Pun.Simple.DISABLED
{
    public class SimpleOVRGrabber : MonoBehaviour {}
}
#else

namespace Photon.Pun.Simple
{
    public class SimpleOVRGrabber : OVRGrabber
    {

    }
}

#endif