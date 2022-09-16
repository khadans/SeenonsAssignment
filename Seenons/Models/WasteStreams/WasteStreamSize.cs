namespace Seenons.WasteStreams
{
    public class WasteStreamSize
    {
        public int Id { get; set; }
        public short Size { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal UnitPricePickup { get; set; }
        public decimal UnitPriceRent { get; set; }
        public decimal UnitPricePlacement { get; set; }
        public Container Container { get; set; }
    }
}
