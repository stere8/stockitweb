namespace StockIT.BLL.Exceptions;

public class CategoryServiceException : Exception
{
    public CategoryServiceException(string message) : base(message) { }
}