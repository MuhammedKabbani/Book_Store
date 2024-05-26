namespace EntityLayer.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(int id) : base ($"Book with id: {id} could not be found.")
        {
            
        }
    }

}
