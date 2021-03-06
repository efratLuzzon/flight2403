﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace flight.Model
{
    public interface lFlightModel : INotifyPropertyChanged
    {
        double IndicatedHeading { get; set; }
        double GpsVertical { get; set; }
        double GpsGround { get; set; }
        double Airspeed { get; set; }
        double GpsAltitude { get; set; }
        double Pitch { get; set; }
        double PitchDeg { get; set; }
        double Altimeter { get; set; }
        double LatitudeDeg { get; set; }
        double LongitudeDeg { get; set; }
        Location LocationF { get; set; }
        string Error { get; set; }
        void Connect(string ip, int port);
        void Disconnect();
        void StartGet();
        void StartSet();
        void StartErrors();
        void UpdateControlParameter(string command);

    }
}
