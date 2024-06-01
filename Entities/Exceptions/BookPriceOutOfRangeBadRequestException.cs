namespace EntityLayer.Exceptions
{
    public class BookPriceOutOfRangeBadRequestException : BadRequestException
    {
        public BookPriceOutOfRangeBadRequestException() : base("Max Price should be less than 1000 and greater than 25")
        {

        }
    }
}
