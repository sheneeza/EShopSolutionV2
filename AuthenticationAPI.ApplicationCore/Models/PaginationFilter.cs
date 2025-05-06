using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.ApplicationCore.Models;

public class PaginationFilter
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}