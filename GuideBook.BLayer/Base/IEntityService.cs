namespace GuideBook.BLayer;

public interface IEntityService<T, DtoType> : IService, IDTOService<DtoType> where T : class
{
    T ConvertToBO(DtoType dto);
    T ConvertToBO(T bo, DtoType dto);
}
