using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;
using UserRoles.Dtos.RequestDtos;
using UserRoles.Models;
using UserRoles.Models.Traveller;
using UserRoles.Models.Trek;
using UserRoles.Services.Interface;
using TrekFAQ = UserRoles.Models.Trek.TrekFAQ;
using TrekItineraryDay = UserRoles.Models.Trek.TrekItineraryDay;
using TrekPackageCostInfo = UserRoles.Models.Trek.TrekPackageCostInfo;
using TrekPackageFixedPricing = UserRoles.Models.Trek.TrekPackageFixedPricing;
using TrekPackageGroupPricing = UserRoles.Models.Trek.TrekPackageGroupPricing;
using TrekFAQDto = UserRoles.Dtos.RequestDtos.TrekFAQ;
using TrekItineraryDayDto = UserRoles.Dtos.RequestDtos.TrekItineraryDay;
using TrekPackageCostInfoDto = UserRoles.Dtos.RequestDtos.TrekPackageCostInfo;
using TrekPackageFixedPricingDto = UserRoles.Dtos.RequestDtos.TrekPackageFixedPricing;
using TrekPackageGroupPricingDto = UserRoles.Dtos.RequestDtos.TrekPackageGroupPricing;

namespace UserRoles.Controllers
{


    public class TrekPackageController : Controller
    {
        public readonly IFileService _fileService;
        public readonly ITrekAppService _trekPackageService;
        private readonly StripeSettings _stripeSettings;




        public TrekPackageController(IFileService fileService, ITrekAppService trekPackageService, IOptions<StripeSettings> stripeOptions)
        {
            _fileService = fileService;
            _trekPackageService = trekPackageService;
            _stripeSettings = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }


        [Route("trek/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var trekPackage = await _trekPackageService.GetTrekPackageBySlugAsync(slug, true, true, true, true);

            if (trekPackage == null)
                return NotFound();


            return View(trekPackage); // Pass a single TrekPackage
        }

        private TrekPackageRequestDto MapToTrekPackageRequestDto(TrekPackage trekPackage)
        {
            var dto = new TrekPackageRequestDto
            {
                Id = trekPackage.Id,
                Title = trekPackage.Title,
                Slug = trekPackage.Slug,
                Description = trekPackage.Description,
                Country = trekPackage.Country,
                Duration = trekPackage.Duration,
                Difficulty = trekPackage.Difficulty,
                Activity = trekPackage.Activity,
                MaxAltitude = trekPackage.MaxAltitude,
                BestSeason = trekPackage.BestSeason,
                Accomodation = trekPackage.Accomodation,
                Meal = trekPackage.Meal,
                StartEndPoint = trekPackage.StartEndPoint,
                TrekOverview = trekPackage.TrekOverview,
                TrekPackingList = trekPackage.TrekPackingList,
                TrekingPackageInclusion = trekPackage.TrekingPackageInclusion,
                TrekingPackageExclusion = trekPackage.TrekingPackageExclusion,
                TrekHighlight = trekPackage.TrekHighlight,
                TrekVideoUrl = trekPackage.TrekVideoUrl,
                PackageCostInfo = new TrekPackageCostInfoDto()
                {
                    Id = trekPackage.PackageCostInfo?.Id ?? 0,
                    BasePrice = trekPackage.PackageCostInfo?.BasePrice ?? 0,
                    Currency = trekPackage.PackageCostInfo?.Currency,
                    PriceNote = trekPackage.PackageCostInfo?.PriceNote,
                    GroupPricing = trekPackage.PackageCostInfo?.GroupPricing?.Select(gp => new TrekPackageGroupPricingDto() {
                    
                        Id = gp.Id,
                        MinPeople = gp.MinPeople,
                        MaxPeople = gp.MaxPeople,
                        PricePerPerson = gp.PricePerPerson
                    }).ToList() ?? new List<TrekPackageGroupPricingDto>(),
                    TrekPackageId = trekPackage.Id
                },
                FAQs = trekPackage.FAQs?.Select(faq => new TrekFAQDto() {
                
                    Id = faq.Id,
                    Category = faq.Category,
                    Question = faq.Question,
                    Answer = faq.Answer,
                    CreatedAt = faq.CreatedAt,
                    UpdatedAt = faq.UpdatedAt
                }).ToList() ?? new List<TrekFAQDto>(),
                TrekItineraryDays = trekPackage.trekItineraryDays?.Select(it => new TrekItineraryDayDto()
                {
                    Id = it.Id,
                    DayNumber = it.DayNumber,
                    Title = it.Title,
                    Description = it.Description
                }).ToList() ?? new List<TrekItineraryDayDto>(),
                trekPackageFixedPricings = trekPackage.trekPackageFixedPricings?.Select(fp => new TrekPackageFixedPricingDto() { 
                
                    Id = fp.Id,
                    FromDate = fp.FromDate,
                    ToDate = fp.ToDate,
                    PricePerPerson = fp.PricePerPerson,
                    TrekPackageId = fp.TrekPackageId
                }).ToList() ?? new List<TrekPackageFixedPricingDto>(),
                Image = trekPackage.TrekPackageImages?.Select(img => new TrekPackageImageRequestDto
                {
                    Id = img.Id,
                    Image = img.Image, // This is the image path or URL
                    ImageType = img.ImageType,
                    Caption = img.Caption,
                    SubCaption = img.SubCaption
                    // Note: ImageFiles (IFormFile) is null here as it's not needed for display
                }).ToList() ?? new List<TrekPackageImageRequestDto>()
            };

            return dto;
        }

        public async Task<IActionResult> BookNow(string slug)
        {
            var trekPackage = await _trekPackageService.GetTrekPackageBySlugAsync(slug);

            if (trekPackage == null)
            {
                return NotFound();
            }

            ViewBag.Title = trekPackage.Title;
            ViewBag.TripDuration = trekPackage.Duration;
            ViewBag.Price = trekPackage.PackageCostInfo?.BasePrice ?? 0m;
            ViewBag.Currency = trekPackage.PackageCostInfo?.Currency ?? "USD";
            ViewBag.Slug = slug;
            ViewBag.StripePublishableKey = _stripeSettings.PublishableKey;
            ViewBag.PriceMap = trekPackage.PackageCostInfo?.GroupPricing?.Select(gp => new
            {
                min = gp.MinPeople,
                max = gp.MaxPeople,
                price = gp.PricePerPerson
            }).ToList();

            return View();
        }

		public async Task<IActionResult> FixedBooking(int id)
		{
			var trekPackage = await _trekPackageService.GetFixedTrekPricing(id);

			if (trekPackage == null)
			{
				return NotFound();
			}

			ViewBag.FromDate = trekPackage.FromDate;
			ViewBag.ToDate = trekPackage.ToDate;
			//ViewBag.Title = trekPackage.Title;
			//ViewBag.TripDuration = trekPackage.Duration;
			ViewBag.Price = decimal.Parse(trekPackage.PricePerPerson);

			//ViewBag.Currency = trekPackage.PackageCostInfo?.Currency ?? "USD";
			//ViewBag.Slug = slug;
			ViewBag.StripePublishableKey = _stripeSettings.PublishableKey;
			

			return View();
		}



		[HttpPost]
        public async Task<IActionResult> BookAndPay([FromBody] TripBookingRequest booking)
        {
            try
            {

                if (booking == null || booking.Traveler == null)
                    return BadRequest("Invalid booking data");

                // 1️⃣ Save booking data to DB  
                // Example: _context.Bookings.Add(new Booking { ... });
                // await _context.SaveChangesAsync();

                // 2️⃣ Send confirmation email
                var emails = string.Join(",", booking.Traveler.Email);
                // Example: using IEmailService.SendBookingConfirmationAsync(emails, booking);

                // 3️⃣ Create Stripe session
                var domain = "https://localhost:5001"; // Replace with your domain
                var sessionOptions = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = booking.PricePerPerson * 100, // in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Trek Booking",
                        Description = $"Booking for {booking.PeopleCount} travelers from {booking.StartDate} to {booking.EndDate}"
                    }
                },
                Quantity = booking.PeopleCount
            }
        },
                    Mode = "payment", // ✅ required
                    SuccessUrl = domain + "/Payment/Success?session_id={CHECKOUT_SESSION_ID}", // ✅ required
                    CancelUrl = domain + "/Payment/Cancel", // ✅ required
                    CustomerEmail = booking.Traveler.Email
                };

                var service = new SessionService();
                Session session = service.Create(sessionOptions);

                return Json(new { sessionId = session.Id });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Create()
        {
            var viewModel = new TrekPackageRequestDto();

            return View("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrekPackageRequestDto viewModel)
        {
            try
            {
                // Check if slug already exists
                if (await _trekPackageService.SlugExistsAsync(viewModel.Slug))
                {
                    ModelState.AddModelError("Slug", "This slug already exists. Please use a different one.");
                    return View("Create", viewModel);
                }

                // Map to entity
                var trekPackage = await MapViewModelToEntityAsync(viewModel);

                // Save to DB
                await _trekPackageService.CreateTrekPackageAsync(trekPackage);

                TempData["Success"] = "Trek package created successfully!";
                return RedirectToAction("Create", new { id = trekPackage.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating trek package: {ex.Message}");
            }

            return View("Create", viewModel);
        }


        public async Task<IActionResult> Index()
        {
            var model = await _trekPackageService.Getall();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }




        public async Task<IActionResult> Edit(string slug)
        {
            var model = await _trekPackageService.GetTrekPackageBySlugAsync(slug, true, true, true, true);
            if (model == null)
            {
                return NotFound();
            }

            var trekPackageDto = MapToTrekPackageRequestDto(model);
            return View(trekPackageDto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(TrekPackageRequestDto model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            try
            {
                var trekPackage = await MapViewModelToEntityAsync(model);
                await _trekPackageService.UpdateTrekPackageAsync(trekPackage);
                TempData["SuccessMessage"] = "Trek package updated successfully.";
                return RedirectToAction("Index", "TrekPackage");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating trek package: {ex.Message}");
                return View(model);
            }
        }


        private async Task<TrekPackage> MapViewModelToEntityAsync(TrekPackageRequestDto viewModel)
        {
            var trekPackage = new TrekPackage
            {
                Title = viewModel.Title,
                Slug = viewModel.Slug,
                Description = viewModel.Description,
                Country = viewModel.Country,
                Duration = viewModel.Duration,
                Difficulty = viewModel.Difficulty,
                Activity = viewModel.Activity,
                MaxAltitude = viewModel.MaxAltitude,
                BestSeason = viewModel.BestSeason,
                Accomodation = viewModel.Accomodation,
                Meal = viewModel.Meal,
                StartEndPoint = viewModel.StartEndPoint,
                TrekOverview = viewModel.TrekOverview,
                TrekPackingList = viewModel.TrekPackingList,
                TrekingPackageExclusion = viewModel.TrekingPackageExclusion,
                TrekingPackageInclusion = viewModel.TrekingPackageInclusion,
                TrekHighlight = viewModel.TrekHighlight,
                TrekVideoUrl = viewModel.TrekVideoUrl,  

            };



//if (viewModel.mapImage != null && viewModel.mapImage.Any())
//            {
//                trekPackage.TrekPackageMapImages = new List<TrekPackageMapImage>();

//                foreach (var file in viewModel.mapImage)
//                {
//                    var imagePath = await _fileService.SaveImageAsync(file.mapImage, "uploads/trek-images");

//                    trekPackage.TrekPackageMapImages.Add(new TrekPackageMapImage
//                    {
//                        TrekPackage = trekPackage,
//                        MapImage = imagePath,
//                        Caption = file.Caption
//                    });
//                }
//            }

            if (viewModel.PackageCostInfo != null)
            {
                trekPackage.PackageCostInfo = new TrekPackageCostInfo
                {
                    TrekPackage = trekPackage,
                    BasePrice = viewModel.PackageCostInfo.BasePrice,
                    Currency = viewModel.PackageCostInfo.Currency,
                    PriceNote = viewModel.PackageCostInfo.PriceNote,
                    GroupPricing = viewModel.PackageCostInfo.GroupPricing?.Select(cat => new TrekPackageGroupPricing
                    {
                        MinPeople = cat.MinPeople,
                        MaxPeople = cat.MaxPeople,
                        PricePerPerson = cat.PricePerPerson
                    }).ToList() ?? new List<TrekPackageGroupPricing>()
                };
            }

            if (viewModel.FAQs?.Any(faq => !string.IsNullOrWhiteSpace(faq.Question)) == true)
            {
                trekPackage.FAQs = viewModel.FAQs
                    .Where(faq => !string.IsNullOrWhiteSpace(faq.Question))
                    .Select(faq => new TrekFAQ
                    {
                        TrekPackage = trekPackage,
                        Category = faq.Category,
                        Question = faq.Question,
                        Answer = faq.Answer
                    }).ToList();
            }



            if (viewModel.trekPackageFixedPricings != null && viewModel.trekPackageFixedPricings.Any())
            {
                trekPackage.trekPackageFixedPricings = new List<TrekPackageFixedPricing>();

                foreach (var trekPackageFixedPricings in viewModel.trekPackageFixedPricings)
                {
                    trekPackage.trekPackageFixedPricings.Add(new TrekPackageFixedPricing
                    {
                        TrekPackage = trekPackage,
                        FromDate = trekPackageFixedPricings.FromDate,
                        ToDate = trekPackageFixedPricings.ToDate,
                        PricePerPerson = trekPackageFixedPricings.PricePerPerson



                    });
                }
            }


            if (viewModel.TrekItineraryDays != null && viewModel.TrekItineraryDays.Any())
            {
                trekPackage.trekItineraryDays = new List<TrekItineraryDay>();

                foreach (var itineraryDays in viewModel.TrekItineraryDays)
                {
                    trekPackage.trekItineraryDays.Add(new TrekItineraryDay
                    {
                        TrekPackage = trekPackage,

                        DayNumber = itineraryDays.DayNumber,
                        Description = itineraryDays.Description,
                        Title = itineraryDays.Title,

                    });
                }
            }



            // ✅ Handle Gallery Images Upload
            if (viewModel.Image != null && viewModel.Image.Any())
            {
                trekPackage.TrekPackageImages = new List<TrekPackageImage>();

                foreach (var file in viewModel.Image)
                {
                    var imagePath = await _fileService.SaveImageAsync(file.ImageFiles, "uploads/trek-images");

                    trekPackage.TrekPackageImages.Add(new TrekPackageImage
                    {
                        TrekPackage = trekPackage,
                        Image = imagePath,
                        Caption = file.Caption,
                        SubCaption = file.SubCaption,
                        ImageType =file.ImageType
                    });
                }
            }


            return trekPackage;
        }

    }
}



