using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda.Ice.Shared;

public class IceResponse<T>
{
    public bool IsSuccess { get; set; } = false;

    public T? Data { get; set; }

    private string? _message;

    public string Message
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_message))
            {
                _message = IsSuccess ? "操作成功" : "操作失败";
            }

            return _message;
        }
        set { _message = value; }
    }
}

public class IceResponse : IceResponse<object>
{
}