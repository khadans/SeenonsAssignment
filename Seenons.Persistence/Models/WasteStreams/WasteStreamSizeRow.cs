namespace Seenons.Persistence.WasteStreams.Models
{
    public class WasteStreamSizeRow
    {
        public int id { get; set; }
        public short size { get; set; }
        public int container_product_id { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal unit_price_pickup { get; set; }
        public decimal unit_price_rent { get; set; }
        public decimal unit_price_placement { get; set; }
        public string container_name { get; set; }
        public string container_type { get; set; }
    }
}
