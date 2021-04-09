using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class PlayerChangeEvent : UnityEvent<Dictionary<string, float>> {}
}