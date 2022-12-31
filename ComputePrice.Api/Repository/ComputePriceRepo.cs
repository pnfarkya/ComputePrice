using ComputePrice.Api.Utils;
using ComputePrice.Data;
using ComputePrice.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComputePrice.Repository
{
    public interface IComputePriceRepo
    {
        PriceModel GetComputedPrice(PriceInputRequest request);
        List<PriceModel> GetAll();
    }

    public class ComputePriceRepo : IComputePriceRepo
    {
        private ComputPriceDbContext _dbContext;
        public ComputePriceRepo(ComputPriceDbContext dbContext)
        {
            _dbContext = dbContext;

        }


        /// <summary>
        /// Compute the price by given input and update the database if computed price no exisiting
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PriceModel GetComputedPrice(PriceInputRequest request)
        {
            try
            {
                if (!ComputePriceCache.CalcuatedPrices.Any())
                {
                    LoadCache();
                }

                var prices = _dbContext.PriceModel.FromSqlRaw($"Exec [db_owner].[get_price_by_range] @pricebegdate='{request.PriceDateBeg}', @priceenddate='{request.PriceDateEnd}', @deliverybegdate='{request.DeliveryDateBeg}', @deliveryenddate='{request.DeliveryDateEnd}'").ToList();
                if (prices.Any())
                {
                    var avgPrice = prices.Select(c => c.Price).Average();

                    var priceKey = BuildPriceKey(request.PriceDate, request.DeliveryDate, avgPrice);

                    if (ComputePriceCache.CalcuatedPrices.ContainsKey(priceKey))
                        return ComputePriceCache.CalcuatedPrices[priceKey];

                    _dbContext.Database.BeginTransaction();

                    var newPriceModel = new PriceModel()
                    {
                        DeliveryDate = request.DeliveryDate,
                        Price = avgPrice,
                        PriceDate = request.PriceDate,
                        Status = true
                    };

                    var result = _dbContext.PriceModel.Add(newPriceModel);
                    _dbContext.SaveChanges();
                    _dbContext.Database.CommitTransactionAsync();

                    newPriceModel.Surrogate = result.Entity.Surrogate;
                    ComputePriceCache.CalcuatedPrices.Add(priceKey, newPriceModel);

                    return newPriceModel;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured while db operation: {ex.Message}, InnerException: {ex.InnerException}");

                _dbContext.Database.RollbackTransaction();

                throw new Exception($"Exception occured while db operation: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Get All The Exisitng Prices
        /// </summary>
        /// <returns></returns>
        public List<PriceModel> GetAll()
        {
            return _dbContext.PriceModel.ToList();
        }

        private void LoadCache()
        {
            var prices = GetAll();

            foreach (var price in prices)
            {
                var key = BuildPriceKey(price.PriceDate, price.DeliveryDate, price.Price);
                if (!ComputePriceCache.CalcuatedPrices.ContainsKey(key))
                    ComputePriceCache.CalcuatedPrices.Add(key, price);
            }
        }

        private string BuildPriceKey(DateTime priceDate, DateTime deliveryDate, decimal price)
        {
            return $"{priceDate.ToString("ddMMyyy")}-{deliveryDate.ToString("ddMMyyy")}-{price}";
        }
    }
}
