using System;

namespace Warface.Entities.Profiles.Common
{
    public struct Rank
    {
        public static Rank Undefined { get; } = new Rank(isUndefined: true);
        public        int  IntValue  { get; private set; }

        Rank(bool isUndefined)
        {
            IntValue = 0;
        }

        public static Rank FromExperience(int experience)
        {
            return new Rank()
            {
                IntValue = GetRankByExp(experience)
            };
        }

        public static Rank FromRankLevel(int rankLevel)
        {
            return new Rank()
            {
                IntValue = rankLevel
            };
        }

        public override string ToString() => IntValue.ToString();

        public static bool operator ==(Rank x, Rank y)
        {
            return x.IntValue == y.IntValue;
        }

        public static bool operator !=(Rank x, Rank y)
        {
            return !(x == y);
        }

        public static bool operator ==(Rank rank, int intRank)
        {
            return rank.IntValue == intRank;
        }

        public static bool operator !=(Rank rank, int intRank)
        {
            return !(rank == intRank);
        }

        public static bool operator >=(Rank rank, Rank otherRank)
        {
            return rank.IntValue >= otherRank.IntValue;
        }

        public static bool operator <=(Rank rank, Rank otherRank)
        {
            return rank.IntValue <= otherRank.IntValue;
        }

        static int GetRankByExp(int exp) //TODO add new ranks
        {
            if (exp >= 19636800)
                return 84;
            if (exp >= 19068600)
                return 83;
            if (exp >= 18473250)
                return 82;
            if (exp >= 17948250)
                return 81;
            if (exp >= 17364000)
                return 80;
            if (exp >= 16795800)
                return 79;
            if (exp >= 16227600)
                return 78;
            if (exp >= 15659400)
                return 77;
            if (exp >= 15091200)
                return 76;
            if (exp >= 14523000)
                return 75;
            if (exp >= 13954800)
                return 74;
            if (exp >= 13386600)
                return 73;
            if (exp >= 12818400)
                return 72;
            if (exp >= 12250200)
                return 71;
            if (exp >= 11682000)
                return 70;
            if (exp >= 11113800)
                return 69;
            if (exp >= 10545600)
                return 68;
            if (exp >= 9977400)
                return 67;
            if (exp >= 9409200)
                return 66;
            if (exp >= 8841000)
                return 65;
            if (exp >= 8272800)
                return 64;
            if (exp >= 7704600)
                return 63;
            if (exp >= 7136400)
                return 62;
            if (exp >= 6568200)
                return 61;
            if (exp >= 6000000)
                return 60;
            if (exp >= 5431800)
                return 59;
            if (exp >= 5069500)
                return 58;
            if (exp >= 4726000)
                return 57;
            if (exp >= 4400600)
                return 56;
            if (exp >= 4092800)
                return 55;
            if (exp >= 3801900)
                return 54;
            if (exp >= 3527500)
                return 53;
            if (exp >= 3268800)
                return 52;
            if (exp >= 3025300)
                return 51;
            if (exp >= 2796400)
                return 50;
            if (exp >= 2581500)
                return 49;
            if (exp >= 2380000)
                return 48;
            if (exp >= 2191200)
                return 47;
            if (exp >= 2014800)
                return 46;
            if (exp >= 1849900)
                return 45;
            if (exp >= 1696200)
                return 44;
            if (exp >= 1552900)
                return 43;
            if (exp >= 1419700)
                return 42;
            if (exp >= 1296000)
                return 41;
            if (exp >= 1181100)
                return 40;
            if (exp >= 1074800)
                return 39;
            if (exp >= 976400)
                return 38;
            if (exp >= 885500)
                return 37;
            if (exp >= 801600)
                return 36;
            if (exp >= 724400)
                return 35;
            if (exp >= 653400)
                return 34;
            if (exp >= 588100)
                return 33;
            if (exp >= 528400)
                return 32;
            if (exp >= 473700)
                return 31;
            if (exp >= 423700)
                return 30;
            if (exp >= 378000)
                return 29;
            if (exp >= 336500)
                return 28;
            if (exp >= 298700)
                return 27;
            if (exp >= 264400)
                return 26;
            if (exp >= 233300)
                return 25;
            if (exp >= 205200)
                return 24;
            if (exp >= 180000)
                return 23;
            if (exp >= 157200)
                return 22;
            if (exp >= 136700)
                return 21;
            if (exp >= 118400)
                return 20;
            if (exp >= 102000)
                return 19;
            if (exp >= 87400)
                return 18;
            if (exp >= 74500)
                return 17;
            if (exp >= 63000)
                return 16;
            if (exp >= 53000)
                return 15;
            if (exp >= 44100)
                return 14;
            if (exp >= 36300)
                return 13;
            if (exp >= 29600)
                return 12;
            if (exp >= 23800)
                return 11;
            if (exp >= 18800)
                return 10;
            if (exp >= 14600)
                return 9;
            if (exp >= 11000)
                return 8;
            if (exp >= 8100)
                return 7;
            if (exp >= 5800)
                return 6;
            if (exp >= 3900)
                return 5;
            if (exp >= 2400)
                return 4;
            if (exp >= 1400)
                return 3;
            if (exp >= 700)
                return 2;
            if (exp >= 0)
                return 1;
            throw new ArgumentOutOfRangeException();
        }
    }
}