namespace GuideBook.BLayer;

public interface IListService<DtoT>
{
    Task<IList<DtoT>> ToListAsync();
    Task<long> CountAsync();
}
