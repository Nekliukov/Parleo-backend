using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    public class UtilityСontroller : Controller
    {
        private readonly IUtilityService _utilityService;

        public UtilityСontroller(
            IUtilityService utilityService
        )
        {
            _utilityService = utilityService;
        }


    }
}
