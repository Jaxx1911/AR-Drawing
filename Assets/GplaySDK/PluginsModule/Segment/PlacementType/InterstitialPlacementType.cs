// Filename: InterstitialPlacementType.cs
// Purpose:
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 16:45 28/12/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

namespace GplaySDK.Segment.PlacementType
{
    public enum InterstitialPlacementType
    {
        Unknown = 0,
        Any = 1,
        StartLevel = 2,
        EndLevel = 3,
        Resume = 4,
        CloseResume = 5,
        Replay = 6,
    }
}