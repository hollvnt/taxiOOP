﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(int id);
        void Add(T item);
        void Remove(int id);
        void Update(T item);
    }
}
