using GuideBook.Dto;
using GuideBook.Repository.Interfaces;
using System.Linq.Expressions;

namespace GuideBook.BLayer;

public abstract class EntityService<T, DtoT> : IEntityService<T, DtoT> where T : class where DtoT : EntityBaseDto
{
    public readonly IRepository<T> repository;
    public EntityService(IRepository<T> _repository)
    {
        repository = _repository;
    }

    #region GetById
    public virtual Task<T> GetByIdAsync(Guid Id) => repository.GetByIdAsync(Id);
    public virtual async Task<DtoT> GetDTOAsync(Guid Id) => ConvertToDTO(await GetByIdAsync(Id));
    #endregion

    #region IsValid
    public virtual bool IsValid(DtoT dto)
    {
        if (dto == null)
            throw new Exception("DtoIsNull");
        else if (dto.Id == Guid.Empty)
            throw new Exception("DtoIdIsEmpty");

        return true;
    }
    public virtual bool IsValidForCreate(DtoT dto)
    {
        if (IsValid(dto))
        {
        }
        return true;
    }
    public virtual bool IsValidForUpdate(DtoT dto)
    {
        if (IsValid(dto))
        {
        }
        return true;
    }
    #endregion

    #region Update
    public virtual async Task UpdateAsync(DtoT dto)
    {
        if (IsValidForUpdate(dto))
        {
            var bo = await GetByIdAsync(dto.Id);
            ConvertToBO(bo, dto);
            await repository.UpdateAsync(dto.Id, bo);
        }
    }
    #endregion

    #region Create
    public virtual async Task<DtoT> CreateAsync(DtoT dto)
    {
        CreateDtoId(dto);
        if (IsValidForCreate(dto))
        {
            var bo = ConvertToBO(dto);
            await CreateAsync(bo);
            return ConvertToDTO(bo);
        }
        return default;
    }
    public virtual async Task<T> CreateAsync(T bo)
    {
        return await repository.AddAsync(bo);
    }
    public virtual void CreateDtoId(DtoT dto) { if (dto?.Id == Guid.Empty) dto.Id = Guid.NewGuid(); }
    #endregion

    #region Delete
    public virtual async Task<DtoT> DeleteAsync(Guid id) => await DeleteAsync(await GetByIdAsync(id));
    public virtual async Task<DtoT> DeleteAsync(T bo)
    {
        if (bo != null)
        {
            BeforeDeleteBO(bo);
            await repository.DeleteAsync(bo);
        }
        return ConvertToDTO(bo);
    }

    public virtual void BeforeDeleteBO(T bo) { }
    #endregion

    #region Convert
    public virtual T ConvertToBO(DtoT dto) => ConvertToBO((T)Activator.CreateInstance(typeof(T)), dto);
    public abstract T ConvertToBO(T bo, DtoT dto);
    public virtual DtoT ConvertToDTO(T bo) => bo != null ? ToDTO(bo) : default;
    public abstract DtoT ToDTO(T bo);
    #endregion

    #region Dispose
    bool isDisposed;
    public virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
            }
            isDisposed = true;
        }
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~EntityService()
    {
        Dispose(false);
    }
    #endregion
}
