using ComputePrice.Data.Model;
using ComputePrice.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ComputePrice.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputePriceController : ControllerBase
    {

        private readonly ILogger<ComputePriceController> _logger;
        private readonly IComputePriceRepo _repo;
        public ComputePriceController(ILogger<ComputePriceController> logger, IComputePriceRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        /// <summary>
        /// Get All The Exisitng Prices
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPrices")]
        public List<PriceModel> GetAll()
        {
            return _repo.GetAll();
        }

        /// <summary>
        /// Compute the price by given input and update the database if computed price no exisiting
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Compute")]
        public PriceModel GetComputedPrice(PriceInputRequest request)
        {
            return _repo.GetComputedPrice(request);
        }
    }
}