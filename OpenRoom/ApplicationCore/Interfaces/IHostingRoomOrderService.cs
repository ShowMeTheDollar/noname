using ApplicationCore.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHostingRoomOrderService
    {
      
        
     // 這是用 Entity 資料，更好的做法
     
        
        public List<Order> GetHostingRoomOrdersByMember(int memberId);
        //public List<Order> GetAllOrders();


    }


    
}
