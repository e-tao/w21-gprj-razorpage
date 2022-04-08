namespace Products.Model;

public class Product
{
    public int ProductId { get; set; }


    [Required]
    [Display(Name = "Product Name")]
    public string? ProductName { get; set; }

    [Display(Name = "Product Image")]
    public string? ImageLink { get; set; }

    [Required]
    [Display(Name = "Product Type")]
    public ProductType Type { get; set; }

    public PizzaSize Size { get; set; }

    [Required]
    public int Quantity { get; set; }


    [Display(Name = "Gluten Free")]
    public bool IsGlutenFree { get; set; }

    [Required]
    [Display(Name = "Unit Price")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(7,2)")]
    [Range(0.00, 99.99)]
    public decimal Price { get; set; }

    [Display(Name = "Batch No.")]
    public string? BatchNumber { get; set; }

    [Display(Name = "Best Before")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    [DataType(DataType.Date)]
    public DateTime BestBefore { get; set; }

}
public enum PizzaSize { Small, Medium, Large }

public enum ProductType { Pizza, Other }