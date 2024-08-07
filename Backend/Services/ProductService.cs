using Backend.Data;

namespace Backend.Services;

public class ProductService
{
    private readonly IDatabaseHandler _databaseConnection;

    public ProductService(IDatabaseHandler databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category)
    {
        try
        {
            await _databaseConnection.AddProduct(name, price, quantityWeight, category);
        }
        catch (Exception ex) //TODO
        {
            throw ex;
        }
    }
}