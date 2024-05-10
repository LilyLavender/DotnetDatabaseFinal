using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class TradersContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    public void AddCategory(Category category)
    {
        this.Categories.Add(category);
        this.SaveChanges();
    }

    public void AddProduct(Product product)
    {
        this.Products.Add(product);
        this.SaveChanges();
    }

    public void DeleteCategory(Category category)
    {
        this.Categories.Remove(category);
        this.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        this.Products.Remove(product);
        this.SaveChanges();
    }

    public void EditCategory(Category updatedCategory)
    {
        Category category = this.Categories.Find(updatedCategory.CategoryID);
        category.CategoryName = updatedCategory.CategoryName;
        category.Description = updatedCategory.Description;
        this.SaveChanges();
    }

    public void EditProduct(Product updatedProduct)
    {
        Product product = this.Products.Find(updatedProduct.ProductID);
        product.ProductName = updatedProduct.ProductName;
        product.QuantityPerUnit = updatedProduct.QuantityPerUnit;
        product.UnitPrice = updatedProduct.UnitPrice;
        product.ReorderLevel = updatedProduct.ReorderLevel;
        product.Discontinued = updatedProduct.Discontinued;
        product.CategoryID = updatedProduct.CategoryID;
        this.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration =  new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json");
            
        var config = configuration.Build();
        optionsBuilder.UseSqlServer(@config["NorthwindConsole:ConnectionString"]);
    }
}