using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TribiticaMVP.Contracts
{
    public static class APIRoutes
    {
        public const string Base = "api/v1/";

        public static class Goals
        {
            public static class Year
            {
                public const string GetAll =  Base + "goals/year";
                public const string GetById = Base + "goals/year/{id}";
                public const string Post =    Base + "goals/year";
                public const string Put =     Base + "goals/year/{id}";
                public const string Delete =  Base + "goals/year/{id}";
            }

            public static class Week
            {
                public const string GetAll =  Base + "goals/week";
                public const string GetById = Base + "goals/week/{id}";
                public const string Post =    Base + "goals/week";
                public const string Put =     Base + "goals/week/{id}";
                public const string Delete =  Base + "goals/week/{id}";
            }

            public static class Day
            {
                public const string GetAll = Base + "goals/day";
                public const string GetById = Base + "goals/day/{id}";
                public const string Post = Base + "goals/day";
                public const string Put = Base + "goals/day/{id}";
                public const string Delete = Base + "goals/day/{id}";
            }
        }
    }
}
