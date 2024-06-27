using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusWeb.Models;
using NexusWeb.ViewModels;
using System.Data;
using System.Diagnostics;

namespace NexusWeb.Controllers
{
	[Authorize(Roles = "Admin,Retail Store Employee,Technical staff,Accounts Department Officer") ]
	public class AdminController : Controller
	{
		private readonly ILogger<AdminController> _logger;
		private readonly NexusWebAppContext _appContext;

		public AdminController(ILogger<AdminController> logger, NexusWebAppContext appContext)
		{
			_logger = logger;
			_appContext = appContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
            ViewData["TO"]= _appContext.Orders.Count();
			ViewData["TC"] = _appContext.Users.Where(x => x.Level == 0).Count();
            ViewData["TU"] = _appContext.Users.Count();
            ViewData["TB"] = _appContext.Bookings.Count();
            ViewData["TP"] = _appContext.Products.Count();
            ViewData["TCe"] = _appContext.Categories.Count();
			var od= _appContext.Orders.ToList();
			decimal sumO = 0;
			foreach (var item in od)
			{
				sumO = sumO + item.Total;
			}
			ViewData["TM"] = sumO;
            var naxusWebAppContext = _appContext.Bookings.Include(b => b.Connection);
            return View(await naxusWebAppContext.ToListAsync());
        }

		[HttpGet]
		public IActionResult ListConnection() {
            
            return View();
		}

        [HttpPost]
		public IActionResult ListConnection(ConnectionType connectionType)
		{
			var ConnectionType= new ConnectionType();
            ConnectionType.Name= connectionType.Name;
            ConnectionType.Deposit= connectionType.Deposit;

            _appContext.ConnectionTypes.Add(ConnectionType);
			_appContext.SaveChanges();
			ViewData["Message"] = "Add new Connection Success";
            return View();
		}

		[HttpGet]
		public IActionResult ListProduct() {	
			return View();
		
		}
    }
}
