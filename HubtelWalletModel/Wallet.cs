using System;
using System.ComponentModel.DataAnnotations;

namespace HubtelWalletModel;

public class Wallet
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [RegularExpression("momo|card")]
    public string Type { get; set; }

    [Required]
    [MaxLength(16)]
    public string AccountNumber { get; set; }

    [Required]
    [RegularExpression("visa|mastercard|mtn|vodafone|airteltigo")]
    public string AccountScheme { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    [Phone]
    public string Owner { get; set; }
}