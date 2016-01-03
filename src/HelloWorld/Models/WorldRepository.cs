using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld.Models
{
	public class WorldRepository : IWorldRepository
	{
		private WorldContext _context;


		public WorldRepository(WorldContext context)
		{
			_context = context;
		}

		public void AddStop(string tripName, Stop newStop, string userName)
		{
			Trip existingTrip = GetTripByName(tripName, userName);
            newStop.Order = existingTrip.Stops.Any() ? existingTrip.Stops.Max(stop => stop.Order) + 1 : 1;
			existingTrip.Stops.Add(newStop);
			_context.Stops.Add(newStop);
		}

		public void AddTrip(Trip newTrip) => _context.Add(newTrip);


		public IEnumerable<Trip> GetAllTrips()
		{
			try
			{
				return _context.Trips.OrderBy(t => t.Name).ToList();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public IEnumerable<Trip> GetAllTripsWithStops()
		{
			try
			{
				return _context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public Trip GetTripByName(string tripName, string userName) => _context.Trips.Include(t => t.Stops)
																		.Where(t => t.Name == tripName && t.UserName == userName).FirstOrDefault();



		public IEnumerable<Trip> GetUserTripsWithStops(string userName) => _context.Trips.Include(t => t.Stops)
																		.OrderBy(t => t.Name)
																		.Where(t => t.UserName.ToLower() == userName.ToLower()).ToList();


		public bool SaveAll() => _context.SaveChanges() > 0;

	}
}