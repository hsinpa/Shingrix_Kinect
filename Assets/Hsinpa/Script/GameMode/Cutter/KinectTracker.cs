using Hsinpa.KinectWrap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode.Game
{
    public class KinectTracker : ITracker
    {
        CustomBodyView m_customBodyView;
        public KinectTracker(CustomBodyView customBodyView )
        {
            m_customBodyView = customBodyView;
        }

        public TrackerStruct GetTracker(int index)
        {
            return m_customBodyView.GetTrackerStruct((uint)index);
        }

        public List<TrackerStruct> GetTrackers()
        {
            return m_customBodyView.GetTrackerStructs();
        }

        public void OnUpdate()
        {

        }
    }
}