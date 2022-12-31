using ComputePrice.Data.Model;

namespace ComputePrice.Api.Utils
{
    public static class ComputePriceCache
    {
        public static Dictionary<string, PriceModel> CalcuatedPrices = new Dictionary<string, PriceModel>();
    }
}
