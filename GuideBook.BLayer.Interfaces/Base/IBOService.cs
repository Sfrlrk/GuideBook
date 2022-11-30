namespace GuideBook.BLayer;

public interface IBOService<DtoT>
{
    Task<DtoT> GetDTOAsync(Guid Id);

    Task<DtoT> CreateAsync(DtoT dto);
    Task UpdateAsync(DtoT dto);

    Task<DtoT> DeleteAsync(Guid id);
}
