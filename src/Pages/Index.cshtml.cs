using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace src.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}

public class Book
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN {get;set;}
    public int? Erscheinungsjahr {get;set;}
    
    [Range(1, Int32.MaxValue)]
    public int? Auflage {get; set;}
}
