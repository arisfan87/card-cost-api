namespace CardCostApi.Infrastructure.Store
{
    public static class Migrations
    {
        public static string InitialDbSchema => @"
        CREATE TABLE IF NOT EXISTS public.CardCosts
        (
            country text UNIQUE,
            cost integer NOT NULL
        );

        INSERT INTO CardCosts (Cost, Country) 
        VALUES (15, 'US');";
    }
}
