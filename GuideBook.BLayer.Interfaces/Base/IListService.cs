using System.Linq.Expressions;

namespace GuideBook.BLayer;

public interface IListService<DtoT>
{
    Task<IList<DtoT>> ToListAsync();
}
