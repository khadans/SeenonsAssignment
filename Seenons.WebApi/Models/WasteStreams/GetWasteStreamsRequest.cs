using System.ComponentModel.DataAnnotations;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class GetWasteStreamsRequest
    {
        [Required]
        [RegularExpression(@"^[1-9][0-9]{3}", ErrorMessage = "Input parameter is not a valid postal code.")]
        [Display(Name = "postalcode")]
        public string PostalCode { get; set; }

        [Display(Name = "weekdays")]
        public ushort[] Weekdays { get; set; }
    }
}
