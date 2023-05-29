using System.ComponentModel.DataAnnotations.Schema;

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
    }
}