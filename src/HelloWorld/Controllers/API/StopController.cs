using AutoMapper;
using HelloWorld.Models;
using HelloWorld.Services;
using HelloWorld.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HelloWorld.Controllers.API
{
    [Authorize]
    [Route("api/trips/{tripName}/stops"),]
    public class StopController : Controller
    {
        private IWorldRepository _repository;
        private CoordService _coordService;

        public StopController(IWorldRepository repository, CoordService service)
        {
            _repository = repository;
            _coordService = service;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var result = _repository.GetTripByName(tripName,User.Identity.Name);
                return Json(result == null ? null : Mapper.Map<IEnumerable<StopAPIModel>>(result.Stops));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<JsonResult> Post(string tripName, [FromBody]StopAPIModel stop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stop);

                    //Compute latitude and longitude
                    var result = await _coordService.Lookup(newStop.Name);
                    if (!result.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(result.Message);
                    }
                    else
                    {
                        newStop.Longitude = result.Longitude;
                        newStop.Latitude = result.Latitude;
                    }
                    _repository.AddStop(tripName, newStop,User.Identity.Name);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopAPIModel>(newStop));
                    }

                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(null);
            }
            return null;
        }
    }
}
