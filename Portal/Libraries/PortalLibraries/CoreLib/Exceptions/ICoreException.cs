using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public interface ICoreException
    {
        /// <summary>
        /// �������������� ���������.
        /// </summary>
        MLString MLMessage { get; set; }
    }
}
