using System;
using UnityEngine;

namespace Infrastructure
{
    [Serializable]
    public class TimeDataWrapper
    {
        public string Time = "14:33:59";

        public void SetCurrentTime(string resultRequest)
        {
            ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(resultRequest);
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(response.time).LocalDateTime;
            Time = dateTime.ToString(Constants.ParseCurrentFormat);
        }
    }

    public struct ServerTimeResponse
    {
        public long time;
    }
}