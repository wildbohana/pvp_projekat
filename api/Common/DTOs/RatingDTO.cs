﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [DataContract]
    public class RatingDTO
    {
        [DataMember]
        public string? RideId { get; set; }
        [DataMember]
        public int Rate { get; set; }     // u controller-u proveri da li je rating između 1 i 5
        [DataMember]
        public string? DriverId { get; set; }
    }
}
