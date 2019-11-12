namespace Calrom.Training.AuctionHouse.Database
{
    public class BidRepo
    {
        private static BidRepo Instance = null;
        private static readonly object padlock = new object();
        public static BidRepo getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new BidRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private BidDatabaseModel _bidContext;
        public BidRepo()
        {
            _bidContext = new BidDatabaseModel();
        }

        public void Add(int entity)
        {
            _bidContext.UserID = entity;
        }

        public int GetUser()
        {
            return _bidContext.UserID;
        }

        public double GetAmount()
        {
            return _bidContext.Amount;
        }
    }
}
