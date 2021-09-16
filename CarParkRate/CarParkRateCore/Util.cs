using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarParkRateCore
{
    public static class Util
    {
        public static Type[] GetAllEntities()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            return (from System.Type type in types where type.IsSubclassOf(typeof(ParkingRate)) select type).ToArray();
        }
    }
}
