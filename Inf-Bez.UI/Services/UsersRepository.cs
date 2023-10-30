﻿using Actions.Server;
using InfBez.Ui.Models;

namespace InfBez.Ui.Services
{
    public class UsersRepository : CrudRepository<User, int>
    {
        public UsersRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
