using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    public class UnitOfWork : IDisposable
    {
        private Context context;
        private Repository<User> userRepository;
        private Repository <Order> userAuthRepository;
        private Repository <Driver> productsRepository;
        private Repository<Administrator> colorsRepository;


        public UnitOfWork()
        {
            context = new Context();
        }

        public Repository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<User>(context);
                return userRepository;
            }
        }

        public Repository<Order> UserAuthRepository
        {
            get
            {
                if (userAuthRepository == null)
                    userAuthRepository = new Repository<Order>(context);
                return userAuthRepository;
            }
        }

        public Repository<Driver> ProductsRepository
        {
            get
            {
                if (productsRepository == null)
                    productsRepository = new Repository<Driver>(context);
                return productsRepository;
            }
        }
        public Repository<Administrator> ColorsRepository
        {
            get
            {
                if (colorsRepository == null)
                    colorsRepository = new Repository<Administrator>(context);
                return colorsRepository;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
