namespace StockIT.BLL.Exceptions;

public class ProductServiceException : Exception
{
    public ProductServiceException(string message) : base(message) { }
}