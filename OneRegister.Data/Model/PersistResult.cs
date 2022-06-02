using System;
using System.Collections.Generic;

namespace OneRegister.Domain.Model
{
    public class PersistResult
    {
        public PersistResult()
        {
            IsSuccessful = true;
            Errors = new HashSet<string>();
        }
        public PersistResult(string error)
        {
            IsSuccessful = false;
            Errors = new HashSet<string>
            {
                error
            };
        }
        public static PersistResult SuccessWithId(Guid id)
        {
            return new PersistResult { IsSuccessful = true, Id = id };
        }
        public Guid Id { get; set; }
        public bool IsSuccessful { get; set; }
        public ICollection<string> Errors { get; set; }
    }
}
