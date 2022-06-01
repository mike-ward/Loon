using System;
using System.Threading.Tasks;

namespace Loon.Models
{
    public class DonateNagStatusTask
    {
        private const  int donateNagCounterInterval = 120;
        private static int donateNagCounter         = donateNagCounterInterval - 10;

        public static ValueTask Execute(Timeline timeline)
        {
            ArgumentNullException.ThrowIfNull(timeline);

            if (timeline.Settings.Donated)
            {
                return default;
            }

            if (donateNagCounter >= donateNagCounterInterval)
            {
                donateNagCounter = 0;
                timeline.StatusCollection.Insert(0, new DonateNagStatus());
            }
            else
            {
                donateNagCounter++;
            }

            return default;
        }
    }
}