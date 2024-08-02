﻿using Common.DTOs;
using Common.TableEntites;
using System.Runtime.Serialization;

// In-memory model (čuva se u rečniku)

namespace Common.Models
{
    [DataContract]
    public class Rating
    {
        [DataMember]
        public string? Id { get; set; }     // RideId
        [DataMember]
        public int Rate { get; set; }
        [DataMember]
        public string? CustomerId { get; set; }
        [DataMember]
        public string? DriverId { get; set; }

        public Rating(RatingDTO data, string customerId)
        {
            Id = data.RideId;
            Rate = data.Rate;
            DriverId = data.DriverId;
            CustomerId = customerId;
        }

        public Rating(RatingEntity entity)
        {
            Id = entity.Id;
            Rate = entity.Rate;
            CustomerId = entity.CustomerId;
            DriverId = entity.DriverId;
        }
    }
}
