using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// 專用型的
    /// </summary>
    public interface IHostingRoomRepository : IRepository<Room> //通用型的Repository
    {
       
    }
}
