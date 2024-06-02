namespace OpenRoom.Admin.Models;

public class BaseApiResponse
{
    public BaseApiResponse()
    {
        
    }
    public BaseApiResponse(object body) //預計要有值，可設計有參數的建構式，封裝的用法
    {
        Body = body;
        IsSuccess = true;
    }

    //自己寫的回傳，吐出去是api, 如200就會有這兩個，bod用object就好因為JSON serilize...就好
    public bool IsSuccess { get; set; }
    public object Body { get; set; }
    public string Message { get; set; }
}