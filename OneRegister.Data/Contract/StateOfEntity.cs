namespace OneRegister.Data.Contract;

public enum StateOfEntity
{
    Init = 0,
    Pending = 1,
    Complete = 2,
    Fetched = 3,
    Inadequate = 4,
    Deleted = 5,
    InProgress = 6,
    Fail = 7
}
