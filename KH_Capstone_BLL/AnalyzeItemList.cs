using System.Collections.Generic;

namespace KH_Capstone_BLLs
{
    public class AnalyzeItemList
    {
        public static int MostCommonID(List<int> idList)
        {
            int mostCommonID = 0;

            if(idList != null && idList.Count > 0)
            {
                Dictionary<int, int> counts = new Dictionary<int, int>();
                foreach(int id in idList)
                {
                    if(counts.ContainsKey(id))
                    {
                        counts[id]++;
                    }
                    else
                    {
                        counts.Add(id, 1);
                    }
                }

                int highestOccurance = 0;

                foreach(KeyValuePair<int,int> count in counts)
                {
                    if(count.Value > highestOccurance)
                    {
                        mostCommonID = count.Key;
                        highestOccurance = count.Value;
                    }
                }
            }

            return mostCommonID;
        }

        public static int LeastCommonID(List<int> idList)
        {
            int leastCommonID = 0;

            if (idList != null && idList.Count > 0)
            {
                Dictionary<int, int> counts = new Dictionary<int, int>();
                foreach (int id in idList)
                {
                    if (counts.ContainsKey(id))
                    {
                        counts[id]++;
                    }
                    else
                    {
                        counts.Add(id, 1);
                    }
                }

                int lowestOccurance = counts[idList[0]];
                
                foreach (KeyValuePair<int, int> count in counts)
                {
                    if (count.Value < lowestOccurance)
                    {
                        leastCommonID = count.Key;
                        lowestOccurance = count.Value;
                    }
                }
            }

            return leastCommonID;
        }

        public static Dictionary<int,int> ItemDropCount(List<int> idList)
        {
            Dictionary<int, int> itemDropCount = new Dictionary<int, int>();

            if (idList!=null)
            {
                foreach (int id in idList)
                {
                    if (itemDropCount.ContainsKey(id))
                    {
                        itemDropCount[id]++;
                    }
                    else
                    {
                        itemDropCount.Add(id, 1);
                    }
                } 
            }

            return itemDropCount;
        }

        public static Dictionary<string,int> LocationCount(List<string> locationList)
        {
            Dictionary<string, int> locationDictionary = new Dictionary<string, int>();

            foreach(string location in locationList)
            {
                if(locationDictionary.ContainsKey(location))
                {
                    locationDictionary[location]++;
                }
                else
                {
                    locationDictionary.Add(location, 1);
                }

            }

            return locationDictionary;
        }
    }
}