namespace GuideBook.Dto;

public class ServiceResult<T>
{
    public T Data { get; set; }

    public string ResultCode { get; set; }
    public string ResultDescription { get; set; }
    public bool IsSuccess { get; set; }

    public ServiceResult(string resultCode, string resultDesc, T data = default)
    {
        Data = data;
        ResultCode = resultCode;
        ResultDescription = resultDesc;
        IsSuccess = data  != null;
    }
    public ServiceResult()
    {
    }
}
