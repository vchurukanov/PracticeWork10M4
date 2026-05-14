using System;
using System.Text.Json.Serialization;

namespace PracticeWork10M4.Models;

public class RealEstateObject
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int Type { get; set; }

    public double Area { get; set; }

    public int Rooms { get; set; }

    [JsonPropertyName("monthlyRentPrice")]
    public decimal Price { get; set; }

    public decimal SalePrice { get; set; }

    public int RentalStatus { get; set; }

    [JsonPropertyName("ownerFullName")]
    public string OwnerName { get; set; } = string.Empty;

    public string OwnerPhone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string PriceText => $"{Price:0.00} грн";

    public string RentalStatusText => RentalStatus switch
    {
        0 => "Вільний",
        1 => "Орендований",
        2 => "На ремонті",
        3 => "Заброньований",
        _ => "Невідомо"
    };
}