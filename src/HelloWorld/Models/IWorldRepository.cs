using System;
using System.Collections.Generic;

namespace HelloWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        void AddTrip(Trip newTrip);
        bool SaveAll();
        Trip GetTripByName(string tripName,string userName);
        void AddStop(string tripName,Stop newStop,string userName);
        IEnumerable<Trip> GetUserTripsWithStops(string userName);
    }
}
