namespace Script
{
    public struct Statistic
    {
        public int populationMax;
        public int populationEvacuation;
        public int populationEating;

        public Statistic(int populationMax, int populationEvacuation, int populationEating)
        {
            this.populationMax = populationMax;
            this.populationEvacuation = populationEvacuation;
            this.populationEating = populationEating;
        }
    }
    
}