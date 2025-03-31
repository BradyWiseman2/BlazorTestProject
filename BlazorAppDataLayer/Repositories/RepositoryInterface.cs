using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer
{
    public interface IRepository<T, I> //T is the database object, like Category or Product. I refers to the datatype of the Primary Key, usually int or string
    {
        T GetByID(I ID);       
        IEnumerable<T> GetAll();
        void Add(T Item);
        void Update(T Item);
        void Delete(I ID);
    }                      
}
