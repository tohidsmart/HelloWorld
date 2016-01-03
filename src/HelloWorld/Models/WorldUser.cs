using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace HelloWorld.Models
{
    public class WorldUser:IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}