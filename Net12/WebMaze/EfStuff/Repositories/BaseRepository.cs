﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public abstract class BaseRepository<Template> where Template : BaseModel
    {
        protected WebContext _webContext;
        protected DbSet<Template> _dbSet;

        public BaseRepository(WebContext webContext)
        {
            _webContext = webContext;
            _dbSet = webContext.Set<Template>();
        }

        public virtual Template Get(long id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == id);
        }


        public virtual List<Template> GetAll()
        {
            return _dbSet.Where(x => x.IsActive).ToList();
        }

        public virtual void Save(Template model)
        {
            if (model.Id > 0)
            {
                _webContext.Update(model);
            }
            else
            {
                _dbSet.Add(model);
            }
            _webContext.SaveChanges();
        }
        
        public virtual void Remove(long id)
        {
            Remove(Get(id));
        }

        public virtual void Remove(Template model)
        {
            model.IsActive = false;
            Save(model);
        }

        public virtual void Remove(List<Template> models)
        {
            foreach (Template model in models)
            {
                model.IsActive = false;
                _dbSet.Update(model);
            }

            _webContext.SaveChanges();
        }
    }
}
