﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerPal.Model
{
    public interface IService
    {
        void Start();
        void Stop();

        bool IsRunning { get; }
    }
}
