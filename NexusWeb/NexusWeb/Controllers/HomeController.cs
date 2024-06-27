using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusWeb.Helpers;
using NexusWeb.Models;
using NexusWeb.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList;

namespace NexusWeb.Controllers
{
    public class HomeController : Controller
    {

		private readonly NexusWebAppContext _appContext;
        private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, NexusWebAppContext appContext)
        {
            _logger = logger;
			_appContext = appContext;
        }
        public List<CartItem> CartShop => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public Decimal TotalCart(List<CartItem> ListCart) {
            Decimal total = 0;
			foreach (var item in ListCart)
			{
				total = total + item.Total;
			}
			return total;
        }


        public IActionResult Index()
        {
			
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Shop(int? page=1)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var products =
                _appContext.Products.AsNoTracking().OrderBy(x => x.Name);
            PagedList<Product> lst = new PagedList<Product>(products, pageNumber, pageSize);

            return View(lst);
        }

		public IActionResult ProductCategory(int id ,int? page = 1) {
			int pageSize = 4;
			int pageNumber = page == null || page < 0 ? 1 : page.Value;
			var products =
			_appContext.Products.AsNoTracking().Where(x => x.CategoryId == id).OrderBy(x => x.Name);
			PagedList<Product> lst = new PagedList<Product>(products, pageNumber, pageSize);

			return View(lst);
		}
		public IActionResult Product(int id)	
		{
            var productP = _appContext.Products.SingleOrDefault(x => x.Id == id);
            var ImagesP = _appContext.ProductImages.Where(x => x.ProductId == id).ToList();
            if (productP is null) return NotFound();
			
			var ProductImages = new HomeProductDetailViewModels {
            product= productP, images= ImagesP
            };
			return View(ProductImages);
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Cart()
		{
            ViewData["TotalCartShop"] = TotalCart(CartShop);
                return View(CartShop);
        }
        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            int quantity = 1;
            var gioHang = CartShop;
            var item = gioHang.SingleOrDefault(p => p.ProductId == id);
            if (item == null)
            {
                Product product = _appContext.Products.SingleOrDefault(p => p.Id == id);
                if (product == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.AvatarImages ?? string.Empty,
                    Quantity = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Cart");
        }

        // GET: Home/AddToCart/5
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = CartShop;
            var item = gioHang.SingleOrDefault(p => p.ProductId == id);
            if (item == null)
            {
                Product product = _appContext.Products.SingleOrDefault(p => p.Id == id);
                if (product == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.AvatarImages ?? string.Empty,
                    Quantity = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Cart");
        }
		[Route("/UpdateCart", Name = "UpdateCart")]
		[HttpPost]
        public IActionResult UpdateCart(List<CartItem> LCartItem)
        {
            var CartShop1 = CartShop;
            if (LCartItem != null)
            {
                for(int i=0;i<LCartItem.Count() ; i++)
                {
                    if(LCartItem[i].Quantity != CartShop1[i].Quantity)
                    {
                        CartShop1[i].Quantity = LCartItem[i].Quantity;

					}
                   
                }
                HttpContext.Session.Set(MySetting.CART_KEY, CartShop1);
            }
			return RedirectToAction("Cart");
		}

        public IActionResult RemoveCart(int id)
        {
            var gioHang = CartShop;
            var item = gioHang.SingleOrDefault(p => p.ProductId == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
		{
            var id = HttpContext.Session.GetInt32("IdUser");
            if (id != null)
            {
				ViewData["User"] = _appContext.Users.FirstOrDefault(x => x.Id == id);
				ViewData["TotalCartShop"] = TotalCart(CartShop);
				return View(CartShop);
            }
            else
            {
				return RedirectToAction("Cart");
			}
            
		}
        [HttpPost]
		public async Task<IActionResult> Checkout(List<CartItem> cartItems)
		{
            var id = HttpContext.Session.GetInt32("IdUser");
            if (id != null && CartShop.Count() != 0)
            {
                Order O = new Order
                {
                    UserId = id,
                    Total = TotalCart(CartShop),
                    Status = 0,
                    CreateAt = DateOnly.FromDateTime(DateTime.Now)
                };

                _appContext.Orders.Add(O);
                _appContext.SaveChanges();
                var checkId = _appContext.Orders.FirstOrDefault(x => x.UserId == id && x.Total == O.Total);
                foreach (var item in CartShop)
                {
                    OrderItem OI = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        OrderId = checkId.Id,
                    };
                    _appContext.OrderItems.Add(OI);
                    _appContext.SaveChanges();
					CartShop.Remove(item);
				}
				HttpContext.Session.Set(MySetting.CART_KEY, CartShop);
				return RedirectToAction("Orders");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

		public IActionResult BookingForm() {
            ViewData["ConnectionId"] = new SelectList(_appContext.Connections, "Id", "Name");
            return View();
		}

		[HttpPost]
		public IActionResult BookingForm(BookingVM booking)
		{
            if (ModelState.IsValid)
            {
                var Randomkey = MyUtil.GenerateRamdomKey();
                var Pass = "1";
                User NewUser = new User
                {
                    UserName = booking.Phone,
                    Password = Pass.ToMd5Hash(Randomkey),
                    Level = 0,
                    Status = 0,
                    RandomKey = Randomkey,
                    FullName = booking.Name,
                    Address = booking.Address,
                    Phone = booking.Phone,
                    Image = "",
                };

                _appContext.Users.Add(NewUser);
                _appContext.SaveChanges();

                var checkId= _appContext.Users.FirstOrDefault(x => x.UserName==booking.Phone);
                Booking NewBooking = new Booking
                {
                    Name = booking.Name,
                    UserId = checkId.Id,
                    ConnectionId = booking.ConnectionId,
                    Address = booking.Address,
                    Phone = booking.Phone,
                    Message = booking.Message,
                    Status = 0,
                };

                _appContext.Bookings.Add(NewBooking);
                _appContext.SaveChanges();

				return RedirectToAction("Index");
			}
			ViewData["ConnectionId"] = new SelectList(_appContext.Connections, "Id", "Name");
			return View(booking);
		}


		[Authorize(Roles = "Customer")]
		// Controllers Acount
		public IActionResult AccountD()
		{
            var id = HttpContext.Session.GetInt32("IdUser");
            if (id != null)
            {
                User U= _appContext.Users.FirstOrDefault(x=> x.Id==id);
				return View(U);
			}
            else
            {
				return RedirectToAction("Index");
			}
			
		}
		[Authorize(Roles = "Customer")]
		public IActionResult Dashboard()
		{
			return View();
		}
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Orders()
		{
			var id = HttpContext.Session.GetInt32("IdUser");
			var nexusWebAppContext = _appContext.Orders.Where(x=>x.UserId==id).Include(o => o.User);
			return View(await nexusWebAppContext.ToListAsync());
		}
		[Authorize(Roles = "Customer")]
		public IActionResult OrdersSingle()
		{
			return View();
		}
		[Authorize(Roles = "Customer")]
		public IActionResult Addresses()
		{
            var id = HttpContext.Session.GetInt32("IdUser");
            if (id != null)
            {
                User U = _appContext.Users.FirstOrDefault(x => x.Id == id);
                return View(U);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
		[Authorize(Roles = "Customer")]
		public IActionResult AddressesEdit()
		{
			return View();
		}

		// End Controller Acount

		[Route("/NotFound")]
		public IActionResult PageNotFound()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
