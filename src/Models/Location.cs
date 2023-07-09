using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace src.Models {
    public class Location
    {  
        public int Id {get;set;}
        public string? name {get;set;}
        public string? description {get;set;}

        [ForeignKey("Location")]
        public bool isPlaceToEat {get; set;}

        [ForeignKey("Location")]
        public bool isPlaceToGetFood {get; set;}

        // optional vars
        public string? diet {get; set;}
        public string? dish {get; set;}
        public string? amenity {get; set;}
        public string? cuisine {get; set;}        
        public double? latitude {get; set;}
        public double? longditude {get; set;}
        public string? website_adress {get; set;}
        public string? address_number {get; set;}
        public string? address_street {get; set;}

        public static implicit operator Location(SelectList v)
        {
            throw new NotImplementedException();
        }
    }
}