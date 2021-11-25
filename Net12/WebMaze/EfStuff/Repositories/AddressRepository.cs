using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class AddressRepository
    {
        private WebContext _webContext;

        public AddressRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public StoresAndAddresses Get(long id)
        {
            return _webContext.StoreAddress.SingleOrDefault(x => x.Id == id);
        }

        public List<StoresAndAddresses> GetAll()
        {
            return _webContext
                .StoreAddress
                .Where(x => x.IsActive)
                .ToList();
        }

        public StoresAndAddresses GetRandomStore()
        {
            return _webContext.StoreAddress.First();
        }

        public void Save(StoresAndAddresses address)
        {
            if (address.Id > 0)
            {
                _webContext.Update(address);
            }
            else
            {
                _webContext.StoreAddress.Add(address);
            }

            _webContext.SaveChanges();
        }

        public void Remove(StoresAndAddresses address)
        {
            address.IsActive = false;
            Save(address);
        }

        public void Remove(long addressId)
        {
            var address = Get(addressId);
            Remove(address);
        }
    }
}
