using OneRegister.Data.Context.MasterCard;
using OneRegister.Data.Entities.MasterCard;
using System;
using System.Collections.Generic;

namespace OneRegister.Data.Repository.MasterCard
{
    public class MasterCardRepository
    {
        public MasterCardRepository(MasterCardContext masterCardContext)
        {
            Context = masterCardContext;
        }

        public MasterCardContext Context { get; }
    }
}
