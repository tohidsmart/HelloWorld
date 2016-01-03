using HelloWorld.Models;
using HelloWorld.ViewModels;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNet.Authorization;

namespace HelloWorld.Controllers.API
{
	[Authorize]
	[Route("api/trips")]
	public class TripController : Controller
	{
		private IWorldRepository _repository;

		public TripController(IWorldRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("")]
		public JsonResult Get()
		{
			var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
			return Json(Mapper.Map<IEnumerable<TripAPIModel>>(trips));
		}

		[HttpPost]
		public JsonResult Post([FromBody]TripAPIModel vm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newTrip = Mapper.Map<Trip>(vm);
					newTrip.UserName = User.Identity.Name;
					_repository.AddTrip(newTrip);

					if (_repository.SaveAll())
					{
						Response.StatusCode = (int)HttpStatusCode.Created;
						return Json(Mapper.Map<TripAPIModel>(newTrip));
					}
				}
			}
			catch (Exception ex)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { Messag = ex.Message });
			}

			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(new { Messag = "Failed", Modelstate = ModelState });
		}

	}
}
