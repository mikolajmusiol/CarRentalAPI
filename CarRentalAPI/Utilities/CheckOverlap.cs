namespace CarRentalAPI.Utilities
{
    public static class CheckOverlap
    {
        public static bool OfTwoTimePeriods(DateTime AStart, DateTime AEnd, DateTime BStart, DateTime BEnd)
        {
            return AStart < BEnd && BStart < AEnd;
        }
    }
}
