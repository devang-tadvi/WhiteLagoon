using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;

		public IVillaRepository villa { get; private set; }
		public IVillaNumberRepository villaNumber { get; private set; }

        public IAmenityRepository amenity { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            villa = new VillaRepository(_db);
			villaNumber = new VillaNumberRepository(_db);
			amenity = new AmenityRepository(_db);
        }
		public void Save() {
		_db.SaveChanges();
		}
    }
}
